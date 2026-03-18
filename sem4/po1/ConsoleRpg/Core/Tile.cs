using System.Collections.Generic;
using ConsoleRpg.Entities;
using ConsoleRpg.Items;

namespace ConsoleRpg.Core;

public class Tile(bool isWall)
{
    public bool IsWall { get; set; } = isWall;
    public Player? Player { get; set; }
    private List<IItem> _items = [];
    public char GetSymbol()
    {
        if (Player != null) return '¶';
        if (_items.Count > 0)
        {
            return _items.Count == 1 ? _items[0].Symbol : _items.Count.ToString()[0];
        }
        return IsWall ? '█' : ' ';
    }
    
    public void AddItem(IItem item) => _items.Add(item);
    
    public IItem? GetTopItem() => _items.Count == 0 ? null : _items[^1];
    public List<IItem> GetItems() => _items;
    public IItem? RemoveTopItem()
    {
        if (_items.Count == 0) return null;
        var item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }
}