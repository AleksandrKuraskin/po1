using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public class Equipment : IEquipment
{
    public IItem? LeftHand { get; private set; }
    public IItem? RightHand { get; private set; }

    public IItem? EquipOneHanded(IInventory inventory, IItem? item, bool leftHand, Logger logger)
    {
        var slotIndex = inventory.SelectedIndex;

        if (item != null)
        {
            inventory.RemoveItemAt(slotIndex);
        }

        var oldItem = leftHand ? LeftHand : RightHand;

        if (LeftHand != null && LeftHand == RightHand)
        {
            LeftHand = null;
            RightHand = null;
        }

        if (leftHand) LeftHand = item;
        else RightHand = item;
        
        if (oldItem != null)
        {
            inventory.TryAddItem(oldItem, slotIndex);
            logger.Log(
                item == null ? 
                    $"Unequipped {oldItem.Name}" : 
                    $"Swapped {oldItem.Name} for {item.Name}."
                );
        }
        else
        {
            logger.Log(
                item == null ? 
                    "Can't equip nothing..." : 
                    $"Equipped {item.Name}."
                );
        }
        return null;
    }

    public IItem? EquipTwoHanded(IInventory inventory, IItem? item, Logger logger)
    {
        var slotIndex = inventory.SelectedIndex;

        if (item != null)
        {
            inventory.RemoveItemAt(slotIndex);
        }
        
        var oldLeft = LeftHand;
        var oldRight = RightHand;

        LeftHand = item;
        RightHand = item;

        if (oldLeft != null && oldLeft == oldRight)
        {
            inventory.TryAddItem(oldLeft, slotIndex);
            logger.Log(
                item == null ? 
                    $"Unequipped {oldLeft.Name}" : 
                    $"Swapped {oldLeft.Name} for {item.Name}."
            );
            return null;
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
                    logger.Log($"No space in inventory to add {oldRight.Name}. Dropping this item...", LogType.Warning);
                    return oldRight;
                }
            }
        }
        if(item != null) logger.Log($"Equipped {item.Name}.");
        return null;
    }

    public void SwapHands()
    {
        if (RightHand != LeftHand)
            (LeftHand, RightHand) = (RightHand, LeftHand);
    }

    public int GetTotalDamage()
    {
        var total = 0;
        if (LeftHand != null) total += LeftHand.Stats.Damage.Value;

        if (RightHand != null && RightHand != LeftHand) 
            total += RightHand.Stats.Damage.Value;

        return total;
    }
}