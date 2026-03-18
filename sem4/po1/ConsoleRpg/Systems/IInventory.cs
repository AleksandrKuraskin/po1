using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IInventory
{
    int Capacity { get; }
    int SelectedIndex { get; }
    bool TryAddItem(IItem item, int index = -1);
    IItem? GetItemAt(int index);
    IItem? RemoveItemAt(int index);
    IItem?[] GetItems();
}