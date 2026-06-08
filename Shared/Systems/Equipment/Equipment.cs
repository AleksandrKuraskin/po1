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
            var added = inventory.TryAddItem(oldItem, slotIndex);
            if (!added)
            {
                LogManager.Instance.Log($"No space in inventory to add {oldItem.Name}. Dropping this item...", entity: player.Name, recipientName: player.Name, type: LogType.Warning);
                return oldItem;
            }
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

        IItem? droppedItem = null;

        if (oldLeft != null && oldLeft == oldRight)
        {
            var added = inventory.TryAddItem(oldLeft, slotIndex);
            if (!added)
            {
                LogManager.Instance.Log($"No space in inventory to add {oldLeft.Name}. Dropping this item...", entity: player.Name, recipientName: player.Name, type: LogType.Warning);
                droppedItem = oldLeft;
            }
            else
            {
                LogManager.Instance.Log(
                    item == null ? 
                        $"Unequipped {oldLeft.Name}" : 
                        $"Swapped {oldLeft.Name} for {item.Name}.",
                    entity: player.Name,
                    type: LogType.Action
                );
            }
            return droppedItem;
        }

        var slotUsed = false;

        if (oldLeft != null)
        {
            var added = inventory.TryAddItem(oldLeft, slotIndex);
            if (!added)
            {
                LogManager.Instance.Log($"No space in inventory to add {oldLeft.Name}. Dropping this item...", entity: player.Name, recipientName: player.Name, type: LogType.Warning);
                droppedItem = oldLeft;
            }
            else
            {
                slotUsed = true;
            }
        }

        if (oldRight != null)
        {
            bool added;
            if (!slotUsed)
            {
                added = inventory.TryAddItem(oldRight, slotIndex);
            }
            else
            {
                added = inventory.TryAddItem(oldRight);
            }

            if (!added)
            {
                LogManager.Instance.Log($"No space in inventory to add {oldRight.Name}. Dropping this item...", entity: player.Name, recipientName: player.Name, type: LogType.Warning);
                // If we already dropped oldLeft, this might be a second drop. 
                // In standard gameplay this shouldn't happen often, but for death we handle it.
                // For now, return the most recent drop.
                droppedItem = oldRight; 
            }
        }

        if (item != null) LogManager.Instance.Log($"Equipped {item.Name}.", entity: player.Name, type: LogType.Action);
        return droppedItem;
    }

    public void SwapHands()
    {
        if (RightHand != LeftHand)
            (LeftHand, RightHand) = (RightHand, LeftHand);
    }

    public void Clear()
    {
        LeftHand = null;
        RightHand = null;
    }

    public Dictionary<EquipmentSlot, IItem> GetAllEquipped()
    {
        var dict = new Dictionary<EquipmentSlot, IItem>();
        if (LeftHand != null) dict[EquipmentSlot.LeftHand] = LeftHand;
        if (RightHand != null) dict[EquipmentSlot.RightHand] = RightHand;
        return dict;
    }
}