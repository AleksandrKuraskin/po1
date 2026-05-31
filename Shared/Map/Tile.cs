using System.Collections.Generic;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Map;

public class Tile(bool isWall, int x, int y)
{
    private readonly List<IItem> _items = [];
    
    public bool IsWall { get; set; } = isWall;
    public List<Player> Players { get; } = [];
    public Enemy? Enemy { get; set; }

    public int X { get; } = x;
    public int Y { get; } = y;
    
    public char GetSymbol()
    {
        if (Players.Count > 0) return Players[0].Symbol;
        if (Enemy != null) return Enemy.Symbol;
        if (_items.Count > 0)
        {
            return _items.Count == 1 ? _items[0].Symbol : _items.Count.ToString()[0];
        }
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

}