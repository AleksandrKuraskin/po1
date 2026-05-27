using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Map;

public class Map
{
    private readonly Random _rng = new Random();

    public int Width { get; }
    public int Height { get; }

    private Tile[,] _tiles;
    private readonly HashSet<(int x, int y)> _dirtyTiles = new();

    public Map(int width = 40, int height = 20)
    {
        Width = width;
        Height = height;
        _tiles = new Tile[height, width];
        InitializeTiles();
        
        _tiles[0, 0].IsWall = false;
        _tiles[0, 1].IsWall = false;
        _tiles[1, 0].IsWall = false;
    }

    public void MarkDirty(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            _dirtyTiles.Add((x, y));
        }
    }

    public IEnumerable<(int x, int y)> GetDirtyTiles() => _dirtyTiles;
    public void ClearDirtyTiles() => _dirtyTiles.Clear();

    private void InitializeTiles()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _tiles[y, x] = new Tile(true, x, y);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[y, x];
    }
    
    public void SpawnPlayer(Player player)
    {
        _tiles[player.Y, player.X].Players.Add(player);
        MarkDirty(player.X, player.Y);
    }

    public (int x, int y) GetRandomFreeTile()
    {
        var freeTiles = new List<(int x, int y)>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (!_tiles[y, x].IsWall && _tiles[y, x].Players.Count == 0 && _tiles[y, x].Enemy == null)
                {
                    freeTiles.Add((x, y));
                }
            }
        }

        if (freeTiles.Count == 0) return (0, 0);
        return freeTiles[_rng.Next(freeTiles.Count)];
    }
    
    public bool TryMovePlayer(Player player, int dx, int dy)
    {
        var newX = player.X + dx;
        var newY = player.Y + dy;

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
        {
            LogManager.Instance.Log("You cannot move outside the map.", recipientName: player.Name, type: LogType.Movement);
            return false;
        }
        
        var targetTile = _tiles[newY, newX];
        if (targetTile.IsWall)
        {
            LogManager.Instance.Log("You just bumped into a wall...", recipientName: player.Name, type: LogType.Movement);
            return false;
        }

        MarkDirty(player.X, player.Y);
        _tiles[player.Y, player.X].Players.Remove(player);
        targetTile.Players.Add(player);
        player.SetPosition(newX, newY);
        MarkDirty(newX, newY);
        return true;
    }

    public bool TryMoveEnemy(Enemy enemy, int dx, int dy)
    {
        var newX = enemy.X + dx;
        var newY = enemy.Y + dy;

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
        {
            return false;
        }
        
        var targetTile = _tiles[newY, newX];
        if (targetTile.IsWall || targetTile.Enemy != null)
        {
            return false;
        }

        MarkDirty(enemy.X, enemy.Y);
        _tiles[enemy.Y, enemy.X].Enemy = null;
        targetTile.Enemy = enemy;
        enemy.SetPosition(newX, newY);
        MarkDirty(newX, newY);
        return true;
    }
    
    public List<Enemy> GetAllEnemies()
    {
        var enemies = new List<Enemy>();
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            var e = _tiles[y, x].Enemy;
            if (e != null) enemies.Add(e);
        }
        return enemies;
    }
}
