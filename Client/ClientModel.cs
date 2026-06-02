using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Map;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using ConsoleRpg.Shared.Systems.Network.Dtos;
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
    public GameStateDto? LastState { get; private set; }
    private readonly Dictionary<string, Player> _remotePlayers = new();
    private bool _uiInitialized = false;
    
    public Dictionary<(int x, int y), TileDto> LocalActiveTiles { get; } = new();
    
    private readonly string _serverIp;
    private readonly int _serverPort;
    private bool _isExiting = false;

    public ClientModel(string ip, int port, string playerName, IInputHandler globalInputHandler, List<ActionInfo> globalInstructions)
    {
        _serverIp = ip;
        _serverPort = port;
        Logger = new ConsoleLogger();
        LogManager.Instance.Attach(Logger);
        Player = new Player(0, 0, playerName);
        MapContext = new MapContext { Map = new Map(80, 24) };
        Renderer = new ConsoleRenderer();
        GlobalInputHandler = globalInputHandler;
        GlobalInstructions = globalInstructions;
        CurrentInputState = new MoveState(MapContext, GlobalInputHandler, GlobalInstructions);

        Connect();
    }

    private void Connect()
    {
        try
        {
            _client = new TcpClient(_serverIp, _serverPort);
            _stream = _client.GetStream();
            
            _ = Task.Run(ListenForUpdates);

            var joinMsg = new NetworkMessage("JOIN", Player.Name);
            SendMessage(joinMsg);
        }
        catch (Exception ex)
        {
            LogManager.Instance.Log($"Failed to connect: {ex.Message}", entity: "Client", type: LogType.Error);
        }
    }

    private async Task ListenForUpdates()
    {
        var lengthBuffer = new byte[4];
        while (_client?.Connected == true && !_isExiting)
        {
            try
            {
                if (_stream == null) throw new Exception("Stream is null");
                await _stream.ReadExactlyAsync(lengthBuffer, 0, 4);
                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                var payloadBuffer = new byte[messageLength];
                await _stream.ReadExactlyAsync(payloadBuffer, 0, messageLength);

                var json = Encoding.UTF8.GetString(payloadBuffer);
                var message = JsonSerializer.Deserialize<NetworkMessage>(json);
                
                if (message?.CommandName == "SYNC")
                {
                    var state = JsonSerializer.Deserialize<GameStateDto>(message.Payload);
                    if (state != null) HandleSync(state);
                }
                else if (message?.CommandName == "UPDATE")
                {
                    var update = JsonSerializer.Deserialize<GameUpdateDto>(message.Payload);
                    if (update != null) HandleUpdate(update);
                }
            }
            catch (Exception ex)
            {
                if (_client?.Connected == true && !_isExiting)
                {
                    LogManager.Instance.Log($"Error receiving update: {ex.Message}", entity: "Client", type: LogType.Error);
                }
                break;
            }
        }
        
        if (!_isExiting)
        {
            await TryReconnect();
        }
    }

    private async Task TryReconnect()
    {
        LogManager.Instance.Log("Disconnected from server. Attempting to reconnect...", entity: "Client", type: LogType.Warning);
        Notify();

        for (int i = 1; i <= 10; i++)
        {
            if (_isExiting) return;
            
            await Task.Delay(1000);
            try
            {
                _client?.Close();
                _client = new TcpClient(_serverIp, _serverPort);
                _stream = _client.GetStream();
                
                _ = Task.Run(ListenForUpdates);

                var joinMsg = new NetworkMessage("JOIN", Player.Name);
                SendMessage(joinMsg);
                
                LogManager.Instance.Log("Successfully reconnected!", entity: "Client", type: LogType.Success);
                Notify();
                return;
            }
            catch
            {
                LogManager.Instance.Log($"Reconnection attempt {i}/10 failed...", entity: "Client", type: LogType.Warning);
                Notify();
            }
        }

        LogManager.Instance.Log("Could not reconnect. Exiting game.", entity: "Client", type: LogType.Error);
        Notify();
        await Task.Delay(2000);
        Environment.Exit(0);
    }
    
    private void HandleSync(GameStateDto state)
    {
        LocalActiveTiles.Clear();
        foreach (var t in state.ActiveTiles) LocalActiveTiles[(t.X, t.Y)] = t;
        UpdateFromState(state);
    }
    
    private void HandleUpdate(GameUpdateDto update)
    {
        foreach (var t in update.UpdatedTiles) LocalActiveTiles[(t.X, t.Y)] = t;
        
        if (LastState != null)
        {
            LastState.LocalPlayer = update.LocalPlayer;
            LastState.OtherPlayers = update.OtherPlayers;
            LastState.Logs = update.Logs;
            LastState.IsGameOver = update.IsGameOver;
            LastState.ActiveTiles = LocalActiveTiles.Values.ToList();
            
            UpdateFromState(LastState);
        }
    }

    public void OnStateReceived(GameStateDto state)
    {
        HandleSync(state);
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

    private void UpdateFromState(GameStateDto state)
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
        Player.PlayerNumber = state.LocalPlayer.PlayerNumber;
        Player.X = state.LocalPlayer.X;
        Player.Y = state.LocalPlayer.Y;
        Player.Wallet.GoldValue = state.LocalPlayer.Gold;
        Player.Wallet.CoinValue = state.LocalPlayer.Coins;

        foreach (var (type, stat) in state.LocalPlayer.Stats)
        {
            Player.Stats.GetStat(type).SetBaseValue(stat.BaseValue);
        }
        
        foreach (var log in state.Logs)
        {
            LogManager.Instance.Notify(log);
        }

        if (state.IsGameOver && CurrentInputState is not GameOverState)
        {
            ChangeInputState(new GameOverState(LogFilePath));
        }

        SyncPlayers(state);
        
        Notify();
    }

    private void SyncPlayers(GameStateDto state)
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
        var namesToRemove = _remotePlayers.Keys.Where(name => !currentRemoteNames.Contains(name)).ToList();
        foreach (var name in namesToRemove) _remotePlayers.Remove(name);

        foreach (var dto in state.OtherPlayers)
        {
            if (!_remotePlayers.TryGetValue(dto.Name, out var remotePlayer))
            {
                remotePlayer = new Player(dto.X, dto.Y, dto.Name);
                _remotePlayers[dto.Name] = remotePlayer;
            }

            remotePlayer.Name = dto.Name;
            remotePlayer.PlayerNumber = dto.PlayerNumber;
            remotePlayer.X = dto.X;
            remotePlayer.Y = dto.Y;
            
            foreach (var (type, stat) in dto.Stats)
            {
                remotePlayer.Stats.GetStat(type).SetBaseValue(stat.BaseValue);
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
            LogManager.Instance.Log($"Send error: {ex.Message}", entity: "Client", type: LogType.Error);
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
        _isExiting = true;
        _client?.Close();
        Environment.Exit(0);
    }
}
