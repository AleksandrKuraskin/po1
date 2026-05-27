using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Map;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Currency;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using ConsoleRpg.Shared.Systems.Network;
using ConsoleRpg.Shared.Systems.Network.Dtos;
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
    
    private readonly ConcurrentQueue<(Player player, NetworkMessage message)> _commandQueue = new();

    public Player Player => _clients.Values.FirstOrDefault() ?? new Player(0, 0, "Server");
    public MapContext MapContext => _mapContext;
    private TileDto[] _globalLastTiles = [];
    public ConsoleLogger Logger => _logger;
    public string LogFilePath => _logFilePath;

    public async Task Start()
    {
        _globalLastTiles = new TileDto[_mapContext.Map.Width * _mapContext.Map.Height];
        for (var y = 0; y < _mapContext.Map.Height; y++)
        {
            for (var x = 0; x < _mapContext.Map.Width; x++)
            {
                _globalLastTiles[y * _mapContext.Map.Width + x] = MapTileToDto(_mapContext.Map.GetTile(x, y));
            }
        }

        _ = ServerLoop();

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
    
    private async Task ServerLoop()
    {
        while (_running)
        {
            var stateChanged = false;
            
            lock (_mapContext)
            {
                while (_commandQueue.TryDequeue(out var req))
                {
                    _dispatcher.Dispatch(req.message, this, req.player);
                    stateChanged = true;
                }
                
                ProcessEnemiesTurn();
                
                BroadcastUpdates();
            }
            await Task.Delay(16);
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
                    if (message.CommandName == "JOIN")
                    {
                        lock (_mapContext)
                        {
                            _clients[client] = player;
                            _playerLastLogId[player] = 0;
                            _dispatcher.Dispatch(message, this, player);
                            SendSync(client, player);
                        }
                    }
                    else
                    {
                        _commandQueue.Enqueue((player, message));
                    }
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
    
    private void SendSync(TcpClient client, Player player)
    {
        var activeTiles = new List<TileDto>();
        for (var y = 0; y < _mapContext.Map.Height; y++)
        {
            for (var x = 0; x < _mapContext.Map.Width; x++)
            {
                var tile = _mapContext.Map.GetTile(x, y);
                if (!tile.IsWall || tile.GetItems().Any() || tile.Enemy != null || tile.Players.Any())
                {
                    var dto = MapTileToDto(tile);
                    activeTiles.Add(dto);
                    _globalLastTiles[y * _mapContext.Map.Width + x] = dto;
                }
            }
        }

        var state = new GameStateDto {
            LocalPlayer = MapPlayerToDto(player),
            OtherPlayers = _clients.Values.Where(p => p != player).Select(MapPlayerToDto).ToList(),
            ActiveTiles = activeTiles,
            Logs = GetNewLogs(player),
            Itemized = _mapContext.Itemized,
            Dangerous = _mapContext.Dangerous,
            IsGameOver = !player.Alive
        };
        SendToClient(client, new NetworkMessage("SYNC", JsonSerializer.Serialize(state)));
    }
    
    private void BroadcastUpdates()
    {
        var updatedTiles = new List<TileDto>();
        for (var y = 0; y < _mapContext.Map.Height; y++)
        {
            for (var x = 0; x < _mapContext.Map.Width; x++)
            {
                var newDto = MapTileToDto(_mapContext.Map.GetTile(x, y));
                var idx = y * _mapContext.Map.Width + x;
                
                if (!AreTilesEqual(_globalLastTiles[idx], newDto))
                {
                    updatedTiles.Add(newDto);
                    _globalLastTiles[idx] = newDto;
                }
            }
        }

        foreach (var (client, player) in _clients.ToList())
        {
            var update = new GameUpdateDto {
                LocalPlayer = MapPlayerToDto(player),
                OtherPlayers = _clients.Values.Where(p => p != player).Select(MapPlayerToDto).ToList(),
                UpdatedTiles = updatedTiles,
                Logs = GetNewLogs(player),
                IsGameOver = !player.Alive
            };
            SendToClient(client, new NetworkMessage("UPDATE", JsonSerializer.Serialize(update)));
        }
    }
    
    private void SendToClient(TcpClient client, NetworkMessage msg)
    {
        try
        {
            var json = JsonSerializer.Serialize(msg);
            var data = Encoding.UTF8.GetBytes(json);
            var lengthPrefix = BitConverter.GetBytes(data.Length);
            var stream = client.GetStream();
            stream.Write(lengthPrefix, 0, lengthPrefix.Length);
            stream.Write(data, 0, data.Length);
        }
        catch { }
    }
    
    private bool AreTilesEqual(TileDto a, TileDto b)
    {
        if (a.Symbol != b.Symbol || a.IsWall != b.IsWall || a.EnemyName != b.EnemyName) return false;
        if (a.PlayerNames.Count != b.PlayerNames.Count || a.Items.Count != b.Items.Count) return false;
        
        for (var i = 0; i < a.PlayerNames.Count; i++)
            if (a.PlayerNames[i] != b.PlayerNames[i]) return false;
            
        for (var i = 0; i < a.Items.Count; i++)
            if (a.Items[i].Name != b.Items[i].Name || a.Items[i].Quantity != b.Items[i].Quantity) return false;

        return true;
    }

    public IEnumerable<Player> GetAllPlayers() => _clients.Values;

    private PlayerDto MapPlayerToDto(Player p)
    {
        return new PlayerDto {
            Name = p.Name,
            X = p.X,
            Y = p.Y,
            Gold = p.Wallet.GoldValue,
            Coins = p.Wallet.CoinValue,
            Stats = Enum.GetValues<StatType>().ToDictionary(
                s => s,
                s => new StatDto
                {
                    BaseValue = p.Stats.GetStat(s).BaseValue,
                    Value = p.Stats.GetStat(s).Value
                }),
            Inventory = p.Inventory.GetItems().Select(i => i == null ? null : MapItemToDto(i)).ToList(),
            Equipment = p.Equipment.GetAllEquipped().ToDictionary(kvp => kvp.Key, kvp => MapItemToDto(kvp.Value)!)
        };
    }
    
    private TileDto MapTileToDto(Tile tile)
    {
        return new TileDto {
            X = tile.X,
            Y = tile.Y,
            Symbol = tile.GetSymbol(),
            IsWall = tile.IsWall,
            EnemyName = tile.Enemy?.Name,
            EnemyStats = tile.Enemy?.Stats.GetActiveStatTypes().ToDictionary(s => s, s => new StatDto
            {
                BaseValue = tile.Enemy.Stats.GetStat(s).BaseValue,
                Value = tile.Enemy.Stats.GetStat(s).Value
            }),
            Items = tile.GetItems().Select(MapItemToDto).ToList()!,
            PlayerNames = tile.Players.Select(p => p.Name).ToList()
        };
    }
    
    private ItemDto MapItemToDto(IItem item)
    {
        return item.GetState();
    }
    
    private List<LogEntry> GetNewLogs(Player player)
    {
        var lastLogId = _playerLastLogId.GetValueOrDefault(player, 0);
        var newLogs = LogManager.Instance.GetLogsForPlayer(lastLogId, player.Name).ToList();
        if (newLogs.Count != 0) _playerLastLogId[player] = newLogs.Max(l => l.Id);
        return newLogs;
    }
    public void Attach(IStateObserver observer) => _observers.Add(observer);
    public void Detach(IStateObserver observer) => _observers.Remove(observer);
    public void Notify() { foreach (var obs in _observers) obs.Update(); }

    public void Exit()
    {
        _running = false;
        _listener.Stop();
    }
}
