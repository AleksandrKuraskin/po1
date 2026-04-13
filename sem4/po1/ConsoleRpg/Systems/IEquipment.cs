using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IEquipment
{
    IItem? EquipOneHanded(Player player, IItem? item, bool leftHand, Logger logger);
    IItem? EquipTwoHanded(Player player, IItem? item, Logger logger);
    void SwapHands();
}