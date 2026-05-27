using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Systems;

public interface IEquipment
{
    IItem? EquipOneHanded(Player player, IItem? item, bool leftHand);
    IItem? EquipTwoHanded(Player player, IItem? item);
    void SwapHands();
}