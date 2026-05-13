using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Items;

namespace ConsoleRpg.Model.Systems;

public interface IEquipment
{
    IItem? EquipOneHanded(Player player, IItem? item, bool leftHand);
    IItem? EquipTwoHanded(Player player, IItem? item);
    void SwapHands();
}