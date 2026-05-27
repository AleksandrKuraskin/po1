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
                entity: player.Name,
                type: LogType.Action
                );
        }
        else
        {
            LogManager.Instance.Log(
                item == null ? 
                    "Can't equip nothing..." : 
                    $"Equipped {item.Name}.",
                entity: player.Name,
                recipientName: item == null ? player.Name : null,
                type: item == null ? LogType.Warning : LogType.Action
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
                entity: player.Name,
                type: LogType.Action
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
                    LogManager.Instance.Log($"No space in inventory to add {oldRight.Name}. Dropping this item...", entity: player.Name, recipientName: player.Name, type: LogType.Warning);
                    return oldRight;
                }
            }
        }
        if(item != null) LogManager.Instance.Log($"Equipped {item.Name}.", entity: player.Name, type: LogType.Action);
        return null;
    }

    public void SwapHands()
    {
        if (RightHand != LeftHand)
            (LeftHand, RightHand) = (RightHand, LeftHand);
    }

    public Dictionary<EquipmentSlot, IItem> GetAllEquipped()
    {
        var dict = new Dictionary<EquipmentSlot, IItem>();
        if (LeftHand != null) dict[EquipmentSlot.LeftHand] = LeftHand;
        if (RightHand != null) dict[EquipmentSlot.RightHand] = RightHand;
        return dict;
    }
}