using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Systems;

public class Inventory(int cap = 10) : IInventory
{
    public int Capacity { get; }= cap;
    private readonly IItem?[] _items = new IItem?[cap];
    
    public int SelectedIndex { get; set; }

    public bool TryAddItem(IItem item, int index = -1)
    {
        if (index >= 0 && index < _items.Length)
        {
            if (_items[index] != null) return false;
            _items[index] = item;
            return true;
        }
        for (var i = 0; i < _items.Length; i++)
        {
            if (_items[i] != null)
            {
                continue;
            }
            _items[i] = item;
            return true;
        }
        return false;
    }

    public IItem? GetItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length) return null;
        return _items[slotIndex];
    }

    public IItem? RemoveItemAt(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _items.Length) return null;
        var item = _items[slotIndex];
        _items[slotIndex] = null;
        return item;
    }

    public IItem?[] GetItems() => _items;
}