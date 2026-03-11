using ConsoleRpg.Core;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public class Equipment : IEquipment
{
    public Weapon? LeftHand { get; private set; }
    public Weapon? RightHand { get; private set; }

    public void EquipLeftHand(Weapon weapon)
    {
        LeftHand = weapon;
        if (RightHand != null && RightHand.IsTwoHanded) RightHand = null;
    }

    public void EquipRightHand(Weapon weapon)
    {
        RightHand = weapon;
        if (LeftHand != null && LeftHand.IsTwoHanded) LeftHand = null;
    }

    public void EquipTwoHanded(Weapon weapon)
    {
        LeftHand = weapon;
        RightHand = weapon;
    }

    public bool TryUnequip(Inventory inventory, Weapon weapon, Logger logger)
    {
        if (weapon.IsTwoHanded)
        {
            if (LeftHand == weapon)
            {
                if(!inventory.TryAddItem(LeftHand)) return false;
                LeftHand = null;
            }
            else
            {
                return false;
            }

            if (RightHand == weapon)
            {
                if (!inventory.TryAddItem(RightHand)) return false;
                RightHand = null;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (LeftHand == weapon)
            {
                if(!inventory.TryAddItem(LeftHand)) return false;
                LeftHand = null;
            }

            if (RightHand == weapon)
            {
                if (!inventory.TryAddItem(RightHand)) return false;
                RightHand = null;
            }
        }
        logger.Log($"Weapon {weapon.Name} moved to inventory.");
        return true;
    }

    public void UnequipAll(Inventory inventory, Logger logger)
    {
        if (LeftHand != null) TryUnequip(inventory, LeftHand, logger);
        if (RightHand != null) TryUnequip(inventory, RightHand, logger);
        logger.Log("Unequipped all.");
    }
}