using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Systems;

public class Inventory(int cap = 10) : IInventory
{
    public int Capacity { get; }= cap;
    private readonly IItem?[] _items = new IItem?[cap];
    
    public int SelectedIndex { get; set; }

    public bool TryAddItem(IItem item, int index = -1)
    {
        // If an index is specified, try that first.
        if (index >= 0 && index < _items.Length)
        {
            if (_items[index] == null)
            {
                _items[index] = item;
                return true;
            }
        }

        // If specified index was full or not specified, find the first available slot.
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

    public void Clear()
    {
        for (var i = 0; i < _items.Length; i++) _items[i] = null;
    }
}