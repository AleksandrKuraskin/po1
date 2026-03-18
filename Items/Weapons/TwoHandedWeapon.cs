using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Weapons;

public class TwoHandedWeapon(string name, int maxHealth, int damage) : Weapon
{
    public override string Name { get; } = name;
    public override char Symbol { get; } = 'w';
    
    public override WeaponStats Stats { get; set; } = new WeaponStats(maxHealth, damage);

    public override IItem? TryEquip(IEquipment equipment, IInventory inventory, bool leftHand, Logger logger)
    {
        return equipment.EquipTwoHanded(inventory, this, logger);
    }
}