using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items;

public interface IEquipBehavior
{
    public IItem? Equip(Player player, Weapon weapon, bool leftHand, Logger logger);
}

public class EquipOneHanded : IEquipBehavior
{
    public IItem? Equip(Player player, Weapon weapon, bool leftHand, Logger logger)
    {
        return player.Equipment.EquipOneHanded(player, weapon, leftHand, logger);
    }
}

public class EquipTwoHanded : IEquipBehavior
{
    public IItem? Equip(Player player, Weapon weapon, bool leftHand, Logger logger)
    {
        return player.Equipment.EquipTwoHanded(player, weapon, logger);
    }
}