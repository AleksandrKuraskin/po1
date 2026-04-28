using System.Collections.Generic;
using ConsoleRpg.Entities;
using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;

namespace ConsoleRpg.Core.Map;

public class Tile(bool isWall)
{
    public bool IsWall { get; set; } = isWall;
    public Player? Player { get; set; }
    private readonly List<IItem> _items = [];
    private readonly List<Enemy> _enemies = [];
    public char GetSymbol()
    {
        if (Player != null) return '¶';
        if (_items.Count > 0)
        {
            return _items.Count == 1 ? _items[0].Symbol : _items.Count.ToString()[0];
        }
        if (_enemies.Count > 0) return _enemies[0].Symbol;
        return IsWall ? '█' : ' ';
    }
    
    public void AddItem(IItem item) => _items.Add(item);
    
    public IItem? GetTopItem() => _items.Count == 0 ? null : _items[0];
    public List<IItem> GetItems() => _items;
    public IItem? RemoveTopItem()
    {
        if (_items.Count == 0) return null;
        var item = _items[0];
        _items.RemoveAt(0);
        return item;
    }

    public void AddEnemy(Enemy enemy) => _enemies.Add(enemy);
    public Enemy? GetEnemy() => _enemies.Count == 0 ? null : _enemies[0];

    public void RemoveEnemy()
    {
        if (_enemies.Count == 0) return;
        _enemies.RemoveAt(0);
    }

}