using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IEquipment
{
    IItem? EquipOneHanded(IInventory inventory, IItem? item, bool leftHand, Logger logger);
    IItem? EquipTwoHanded(IInventory inventory, IItem? item, Logger logger);
    void SwapHands();
    int GetTotalDamage();
}