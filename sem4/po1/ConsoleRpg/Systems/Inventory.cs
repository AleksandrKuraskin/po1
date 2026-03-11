using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public class Inventory : IInventory
{
    private readonly IItem?[] _items = new IItem?[10];

    public bool TryAddItem(IItem item)
    {
        for (var i = 0; i < _items.Length; i++)
        {
            if (_items[i] == null)
            {
                _items[i] = item;
                return true;
            }
        }
        return false;
    }

    public IItem? GetItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length) return null;
        return _items[slotIndex];
    }

    public IItem? RemoveItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length) return null;
        var item = _items[slotIndex];
        _items[slotIndex] = null;
        return item;
    }

    public IItem?[] GetItems() => _items;
}