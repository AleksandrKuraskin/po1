using ConsoleRpg.Entities;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IEquipment
{
    IItem? EquipOneHanded(Player player, IItem? item, bool leftHand);
    IItem? EquipTwoHanded(Player player, IItem? item);
    void SwapHands();
}