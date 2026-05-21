using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Map;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using ConsoleRpg.Shared.Systems.Network;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Server;

public class ServerModel(
    CommandDispatcher dispatcher,
    MapContext mapContext,
    ConsoleLogger logger,
    string logFilePath,
    int port) : IServerModel
{
    private readonly List<IStateObserver> _observers = [];
    private readonly Dictionary<TcpClient, Player> _clients = [];
    private readonly Dictionary<Player, long> _playerLastLogId = [];
    private readonly CommandDispatcher _dispatcher = dispatcher;
    private readonly MapContext _mapContext = mapContext;
    private readonly ConsoleLogger _logger = logger;
    private readonly string _logFilePath = logFilePath;
    private readonly TcpListener _listener = new TcpListener(IPAddress.Any, port);
    private bool _running = true;

    public Player Player => _clients.Values.FirstOrDefault() ?? new Player(0, 0, "Server");
    public MapContext MapContext => _mapContext;
    public ConsoleLogger Logger => _logger;
    public string LogFilePath => _logFilePath;

    public async Task Start()
    {
        _listener.Start();
        Console.WriteLine($"Server started on port {((IPEndPoint)_listener.LocalEndpoint).Port}");

        while (_running)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                if (_clients.Count >= 9)
                {
                    client.Close();
                    continue;
                }
                _ = HandleClient(client);
            }
            catch (Exception ex)
            {
                if (_running) Console.WriteLine($"Accept error: {ex.Message}");
            }
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        var lengthBuffer = new byte[4];
        
        var spawn = _mapContext.SpawnPoint;
        var player = new Player(spawn.x, spawn.y, "Connecting...");

        try
        {
            while (_running && client.Connected)
            {
                try
                {
                    await stream.ReadExactlyAsync(lengthBuffer, 0, 4);
                }
                catch (EndOfStreamException)
                {
                    break;
                }
                
                var messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                var payloadBuffer = new byte[messageLength];
                await stream.ReadExactlyAsync(payloadBuffer, 0, messageLength);

                var json = Encoding.UTF8.GetString(payloadBuffer);
                var message = JsonSerializer.Deserialize<NetworkMessage>(json);
                if (message != null)
                {
                    lock (_mapContext)
                    {
                        if (message.CommandName == "JOIN")
                        {
                            _clients[client] = player;
                            var recent = LogManager.Instance.GetRecentLogs(10).ToList();
                            _playerLastLogId[player] = recent.Count > 0 ? recent[0].Id - 1 : 0;
                        }
                        _dispatcher.Dispatch(message, this, player);
                    }
                    BroadcastState();
                }
            }
        }
        catch (Exception ex)
        {
            if (_running && client.Connected)
            {
                Console.WriteLine($"Client error ({player.Name}): {ex.Message}");
            }
        }
        finally
        {
            lock (_mapContext)
            {
                _mapContext.Map.GetTile(player.X, player.Y).Players.Remove(player);
                _clients.Remove(client);
                _playerLastLogId.Remove(player);
                player.RemoveMediator();
            }
            client.Close();
            BroadcastState();
        }
    }

    public void ProcessEnemiesTurn()
    {
        var enemies = _mapContext.Map.GetAllEnemies();
        foreach (var enemy in enemies)
        {
            if (enemy.ActedThisTurn)
            {
                enemy.ActedThisTurn = false;
                continue;
            }
            enemy.TakeTurn(_mapContext.Map);
        }
    }

    public IEnumerable<Player> GetAllPlayers() => _clients.Values;

    private void BroadcastState()
    {
        foreach (var (client, player) in _clients.ToList())
        {
            var state = GetStateForPlayer(player);
            
            var json = JsonSerializer.Serialize(state);
            var data = Encoding.UTF8.GetBytes(json);
            var lengthPrefix = BitConverter.GetBytes(data.Length);
            try
            {
                var stream = client.GetStream();
                stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                stream.Write(data, 0, data.Length);
            }
            catch { /* Client disconnected */ }
        }
    }

    private GameStateDto GetStateForPlayer(Player player)
    {
        var activeTiles = new List<TileDto>();
        for (var y = 0; y < _mapContext.Map.Height; y++)
        {
            for (var x = 0; x < _mapContext.Map.Width; x++)
            {
                var tile = _mapContext.Map.GetTile(x, y);
                if (!tile.IsWall || tile.GetItems().Count > 0 || tile.Enemy != null || tile.Players.Count > 0)
                {
                    activeTiles.Add(new TileDto
                    {
                        X = x, Y = y, 
                        Symbol = tile.GetSymbol(), 
                        IsWall = tile.IsWall,
                        EnemyName = tile.Enemy?.Name,
                        EnemyStats = tile.Enemy?.Stats.GetActiveStatTypes()
                            .ToDictionary(s => s.ToString(), s => tile.Enemy.Stats.GetStat(s).Value) ?? [],
                        ItemNames = tile.GetItems().Select(i => i.Name).ToList(),
                        PlayerNames = tile.Players.Select(p => p.Name).ToList()
                    });
                }
            }
        }

        var lastLogId = _playerLastLogId.GetValueOrDefault(player, 0);
        var newLogs = LogManager.Instance.GetLogsForPlayer(lastLogId, player.Id).ToList();
        if (newLogs.Count != 0)
        {
            _playerLastLogId[player] = newLogs.Max(l => l.Id);
        }

        return new GameStateDto
        {
            LocalPlayer = MapPlayerToDto(player),
            OtherPlayers = _clients.Values.Where(p => p != player).Select(MapPlayerToDto).ToList(),
            ActiveTiles = activeTiles,
            Logs = newLogs,
            Itemized = _mapContext.Itemized,
            Dangerous = _mapContext.Dangerous,
            IsGameOver = !player.Alive
        };
    }

    private PlayerDto MapPlayerToDto(Player p)
    {
        return new PlayerDto
        {
            Id = p.Id,
            Name = p.Name,
            X = p.X,
            Y = p.Y,
            Gold = p.Wallet.GoldValue,
            Coins = p.Wallet.CoinValue,
            Stats = Enum.GetValues<StatType>().ToDictionary(
                s => s.ToString(), 
                s => new StatDto { 
                    BaseValue = p.Stats.GetStat(s).BaseValue, 
                    Value = p.Stats.GetStat(s).Value 
                }),
            Inventory = p.Inventory.GetItems().Where(i => i != null).Select(i => i!.Name).ToList(), // TODO: Convert to item dto
            Equipment = p.Equipment.GetAllEquipped().ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Name)
        };
    }

    public void Broadcast(GameStateDto state) => BroadcastState();
    public void Attach(IStateObserver observer) => _observers.Add(observer);
    public void Detach(IStateObserver observer) => _observers.Remove(observer);
    public void Notify() { foreach (var obs in _observers) obs.Update(); }

    public void Exit()
    {
        _running = false;
        _listener.Stop();
    }
}
