using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using ConsoleRpg.Shared.Systems.Network;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Client.Controller.States;
using ConsoleRpg.Client.Controller.Handlers;
using ConsoleRpg.Client.View;
using ConsoleRpg.Client.View.Components;
using ConsoleRpg.Client.Controller;

namespace ConsoleRpg.Client;

public class ClientModel : IClientModel, INetworkObserver
{
    private readonly List<IStateObserver> _observers = [];
    private TcpClient? _client;
    private NetworkStream? _stream;
    
    public Player Player { get; private set; }
    public MapContext MapContext { get; private set; }
    public ConsoleLogger Logger { get; }
    public string LogFilePath { get; } = "client.log";
    
    public IRenderer Renderer { get; private set; }
    public IInputState CurrentInputState { get; private set; }
    public IInputHandler GlobalInputHandler { get; }
    public List<ActionInfo> GlobalInstructions { get; }
    public GameState? LastState { get; private set; }
    private readonly Dictionary<string, Player> _remotePlayers = new();
    private bool _uiInitialized = false;

    public ClientModel(string ip, int port, string playerName, IInputHandler globalInputHandler, List<ActionInfo> globalInstructions)
    {
        Logger = new ConsoleLogger();
        LogManager.Instance.Attach(Logger);
        Player = new Player(0, 0, playerName);
        MapContext = new MapContext { Map = new Map(80, 24) };
        Renderer = new ConsoleRenderer();
        GlobalInputHandler = globalInputHandler;
        GlobalInstructions = globalInstructions;
        CurrentInputState = new MoveState(MapContext, GlobalInputHandler, GlobalInstructions);

        Connect(ip, port);
    }

    private void Connect(string ip, int port)
    {
        try
        {
            _client = new TcpClient(ip, port);
            _stream = _client.GetStream();
            
            _ = Task.Run(ListenForUpdates);

            var joinMsg = new NetworkMessage("JOIN", Player.Name);
            SendMessage(joinMsg);
        }
        catch (Exception ex)
        {
            LogManager.Instance.Log($"Failed to connect: {ex.Message}", LogType.Error);
        }
    }

    private async Task ListenForUpdates()
    {
        var lengthBuffer = new byte[4];
        while (_client?.Connected == true)
        {
            try
            {
                await _stream!.ReadExactlyAsync(lengthBuffer, 0, 4);
                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                var payloadBuffer = new byte[messageLength];
                await _stream.ReadExactlyAsync(payloadBuffer, 0, messageLength);

                var json = Encoding.UTF8.GetString(payloadBuffer);
                var state = JsonSerializer.Deserialize<GameState>(json);
                if (state != null)
                {
                    OnStateReceived(state);
                }
            }
            catch (Exception ex)
            {
                if (_client?.Connected == true)
                {
                    LogManager.Instance.Log($"Error receiving update: {ex.Message}", LogType.Error);
                }
                break;
            }
        }
    }

    public void OnStateReceived(GameState state)
    {
        UpdateFromState(state);
    }

    private void InitializeUI()
    {
        Renderer.ClearSidebarComponents();
        var components = new List<IUIComponent> { new StatsComponent() };

        if (MapContext.Itemized)
        {
            components.Add(new EquipmentComponent());
            components.Add(new InventoryComponent());
            components.Add(new GroundItemsComponent());
        }

        if (MapContext.Dangerous)
        {
            components.Add(new EnemyComponent());
        }

        components.Add(new PlayersComponent());

        foreach (var component in components)
        {
            Renderer.AddSidebarComponent(component);
        }
        _uiInitialized = true;
    }

    private void UpdateFromState(GameState state)
    {
        var flagsChanged = MapContext.Itemized != state.Itemized || MapContext.Dangerous != state.Dangerous;
        
        MapContext.Itemized = state.Itemized;
        MapContext.Dangerous = state.Dangerous;
        if (!_uiInitialized || flagsChanged)
        {
            InitializeUI();
            if (CurrentInputState is MoveState || !_uiInitialized)
            {
                CurrentInputState = new MoveState(MapContext, GlobalInputHandler, GlobalInstructions);
            }
        }

        LastState = state;
        Player.X = state.LocalPlayer.X;
        Player.Y = state.LocalPlayer.Y;
        Player.Wallet.GoldValue = state.LocalPlayer.Gold;
        Player.Wallet.CoinValue = state.LocalPlayer.Coins;

        foreach (var (key, stat) in state.LocalPlayer.Stats)
        {
            if (Enum.TryParse<StatType>(key, out var type))
            {
                Player.Stats.GetStat(type).SetBaseValue(stat.BaseValue);
            }
        }
        
        foreach (var log in state.Logs)
        {
            if (Logger.GetLogs().All(l => l.Id != log.Id))
            {
                LogManager.Instance.Notify(log);
            }
        }

        if (state.IsGameOver)
        {
            ChangeInputState(new GameOverState(LogFilePath));
        }

        SyncPlayers(state);
        
        Notify();
    }

    private void SyncPlayers(GameState state)
    {
        for (int y = 0; y < MapContext.Map.Height; y++)
        {
            for (int x = 0; x < MapContext.Map.Width; x++)
            {
                MapContext.Map.GetTile(x, y).Players.Clear();
            }
        }

        MapContext.Map.GetTile(Player.X, Player.Y).Players.Add(Player);

        var currentRemoteNames = state.OtherPlayers.Select(p => p.Name).ToHashSet();
        var namesToRemove = _remotePlayers.Keys.Where(n => !currentRemoteNames.Contains(n)).ToList();
        foreach (var name in namesToRemove) _remotePlayers.Remove(name);

        foreach (var dto in state.OtherPlayers)
        {
            if (!_remotePlayers.TryGetValue(dto.Name, out var remotePlayer))
            {
                remotePlayer = new Player(dto.X, dto.Y, dto.Name);
                _remotePlayers[dto.Name] = remotePlayer;
            }

            remotePlayer.X = dto.X;
            remotePlayer.Y = dto.Y;
            
            foreach (var (key, stat) in dto.Stats)
            {
                if (Enum.TryParse<StatType>(key, out var type))
                {
                    remotePlayer.Stats.GetStat(type).SetBaseValue(stat.BaseValue);
                }
            }
            
            MapContext.Map.GetTile(remotePlayer.X, remotePlayer.Y).Players.Add(remotePlayer);
        }
    }

    public void SendMessage(NetworkMessage message)
    {
        if (_stream == null) return;
        try 
        {
            var json = JsonSerializer.Serialize(message);
            var data = Encoding.UTF8.GetBytes(json);
            var lengthPrefix = BitConverter.GetBytes(data.Length);
            _stream.Write(lengthPrefix, 0, lengthPrefix.Length);
            _stream.Write(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            LogManager.Instance.Log($"Send error: {ex.Message}", LogType.Error);
        }
    }

    public void ChangeRenderer(IRenderer newRenderer) => Renderer = newRenderer;
    public void ChangeInputState(IInputState newState) => CurrentInputState = newState;

    public void Attach(IStateObserver observer) => _observers.Add(observer);
    public void Detach(IStateObserver observer) => _observers.Remove(observer);
    public void Notify()
    {
        foreach (var observer in _observers) observer.Update();
    }

    public void Exit()
    {
        _client?.Close();
        Environment.Exit(0);
    }
}
