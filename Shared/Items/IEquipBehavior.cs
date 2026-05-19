using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Shared.Items;

public interface IEquipBehavior
{
    public IItem? Equip(Player player, IItem item, bool leftHand);
}

public class EquipOneHanded : IEquipBehavior
{
    public IItem? Equip(Player player, IItem item, bool leftHand)
    {
        return player.Equipment.EquipOneHanded(player, item, leftHand);
    }
}

public class EquipTwoHanded : IEquipBehavior
{
    public IItem? Equip(Player player, IItem item, bool leftHand)
    {
        return player.Equipment.EquipTwoHanded(player, item);
    }
}