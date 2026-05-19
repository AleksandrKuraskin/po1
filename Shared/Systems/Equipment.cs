using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Systems;

public class Equipment : IEquipment
{
    public IItem? LeftHand { get; private set; }
    public IItem? RightHand { get; private set; }

    public IItem? EquipOneHanded(Player player, IItem? item, bool leftHand)
    {
        var inventory = player.Inventory;
        var slotIndex = inventory.SelectedIndex;

        if (item != null)
        {
            inventory.RemoveItemAt(slotIndex);
        }

        var oldItem = leftHand ? LeftHand : RightHand;

        if (LeftHand != null && LeftHand == RightHand)
        {
            oldItem = LeftHand;
            oldItem.OnUnequip(player);
            LeftHand = null;
            RightHand = null;
        }
        else
        {
            oldItem?.OnUnequip(player);
        }
        
        item?.OnEquip(player);

        if (leftHand) LeftHand = item;
        else RightHand = item;
        
        if (oldItem != null)
        {
            inventory.TryAddItem(oldItem, slotIndex);
            LogManager.Instance.Log(
                item == null ? 
                    $"Unequipped {oldItem.Name}" : 
                    $"Swapped {oldItem.Name} for {item.Name}.",
                LogType.Info, LogScope.Global, null, player.Name
                );
        }
        else
        {
            LogManager.Instance.Log(
                item == null ? 
                    "Can't equip nothing..." : 
                    $"Equipped {item.Name}.",
                item == null ? LogType.Warning : LogType.Info,
                item == null ? LogScope.Private : LogScope.Global,
                item == null ? player.Name : null,
                player.Name
                );
        }
        return null;
    }

    public IItem? EquipTwoHanded(Player player, IItem? item)
    {
        var inventory = player.Inventory;
        var slotIndex = inventory.SelectedIndex;

        if (item != null)
        {
            inventory.RemoveItemAt(slotIndex);
        }
        
        var oldLeft = LeftHand;
        var oldRight = RightHand;

        if (oldLeft != null && oldLeft == oldRight)
        {
            oldLeft.OnUnequip(player);
        }
        else
        {
            oldLeft?.OnUnequip(player);
            oldRight?.OnUnequip(player);
        }
        
        item?.OnEquip(player);
        
        LeftHand = item;
        RightHand = item;

        if (oldLeft != null && oldLeft == oldRight)
        {
            inventory.TryAddItem(oldLeft, slotIndex);
            LogManager.Instance.Log(
                item == null ? 
                    $"Unequipped {oldLeft.Name}" : 
                    $"Swapped {oldLeft.Name} for {item.Name}.",
                LogType.Info, LogScope.Global, null, player.Name
            );
            return null;
        }
        else
        {
            oldLeft?.OnUnequip(player);
            oldRight?.OnUnequip(player);
        }

        var slotUsed = false;

        if (oldLeft != null)
        {
            inventory.TryAddItem(oldLeft, slotIndex);
            slotUsed = true;
        }
        if (oldRight != null)
        {
            if (!slotUsed)
            {
                inventory.TryAddItem(oldRight, slotIndex);
            }
            else
            {
                var added = inventory.TryAddItem(oldRight);
                if (!added)
                {
                    LogManager.Instance.Log($"No space in inventory to add {oldRight.Name}. Dropping this item...", LogType.Warning, LogScope.Private, player.Name, player.Name);
                    return oldRight;
                }
            }
        }
        if(item != null) LogManager.Instance.Log($"Equipped {item.Name}.", LogType.Info, LogScope.Global, null, player.Name);
        return null;
    }

    public void SwapHands()
    {
        if (RightHand != LeftHand)
            (LeftHand, RightHand) = (RightHand, LeftHand);
    }

    public Dictionary<string, IItem> GetAllEquipped()
    {
        var dict = new Dictionary<string, IItem>();
        if (LeftHand != null) dict["LeftHand"] = LeftHand;
        if (RightHand != null) dict["RightHand"] = RightHand;
        return dict;
    }
}