using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IInventory
{
    bool TryAddItem(IItem item);
    IItem? GetItem(int index);
    IItem? RemoveItem(int index);
    IItem?[] GetItems();
}