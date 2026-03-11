using ConsoleRpg.Entities;

namespace ConsoleRpg.Core;

public class Map
{
    private Random _rng = new Random();

    public int Width { get; }
    public int Height { get; }

    private Tile[,] _tiles;

    public Map(int width = 40, int height = 20)
    {
        Width = width;
        Height = height;
        _tiles = new Tile[height, width];
        InitializeTiles();
        GenerateMap(1, 1);
        
        _tiles[0, 0].IsWall = false;
        _tiles[0, 1].IsWall = false;
        _tiles[1, 0].IsWall = false;
    }

    private void InitializeTiles()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                _tiles[y, x] = new Tile(true);
            }
        }
    }

    public Tile GetTile(int x, int y)
    {
        return _tiles[y, x];
    }

    private void GenerateMap(int startX, int startY)
    {
        _tiles[startY, startX].IsWall = false;

        var directions = new List<(int dx, int dy)>
        {
            (0, -2),
            (0, 2),
            (-2, 0),
            (2, 0)
        };

        for (var i = 0; i < directions.Count; i++)
        {
            var r = _rng.Next(i, directions.Count);
            (directions[i], directions[r]) = (directions[r], directions[i]);
        }

        foreach (var dir in directions)
        {
            var nextX = startX + dir.dx;
            var nextY = startY + dir.dy;

            if (nextX > 0 && nextX < Width - 1 && nextY > 0 && nextY < Height - 1 && _tiles[nextY, nextX].IsWall)
            {
                _tiles[startY + dir.dy / 2, startX + dir.dx / 2].IsWall = false;
                GenerateMap(nextX, nextY);
            }
        }
    }
    
    public void SpawnPlayer(Player player)
    {
        _tiles[player.Y, player.X].Player = player;
    }
    
    public bool TryMove(Player player, int dx, int dy)
    {
        var newX = player.X + dx;
        var newY = player.Y + dy;

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height) return false;
        
        var targetTile = _tiles[newY, newX];
        if (targetTile.IsWall || targetTile.Player != null) return false;

        _tiles[player.Y, player.X].Player = null;
        targetTile.Player = player;
        player.SetPosition(newX, newY);
        return true;
    }
}
