using System;
using System.Collections.Generic;
using ConsoleRpg.Entities;
using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Systems.Logging;

namespace ConsoleRpg.Core.Map;

public class Map
{
    private readonly Random _rng = new Random();

    public int Width { get; }
    public int Height { get; }

    private Tile[,] _tiles;

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
    
    public void SpawnPlayer(Player player)
    {
        _tiles[player.Y, player.X].Player = player;
    }
    
    public bool TryMovePlayer(Player player, int dx, int dy)
    {
        var newX = player.X + dx;
        var newY = player.Y + dy;

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
        {
            LogManager.Instance.Log("You cannot move outside the map.");
            return false;
        }
        
        var targetTile = _tiles[newY, newX];
        if (targetTile.IsWall || targetTile.Player != null)
        {
            LogManager.Instance.Log("You just bumped into a wall...");
            return false;
        }

        _tiles[player.Y, player.X].Player = null;
        targetTile.Player = player;
        player.SetPosition(newX, newY);
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

        _tiles[enemy.Y, enemy.X].Enemy = null;
        targetTile.Enemy = enemy;
        enemy.SetPosition(newX, newY);
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
