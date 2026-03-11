using ConsoleRpg.Core;
using ConsoleRpg.Items;

namespace ConsoleRpg.Systems;

public interface IEquipment
{
    void EquipLeftHand(Weapon weapon);
    void EquipRightHand(Weapon weapon);
    void EquipTwoHanded(Weapon weapon);
    bool TryUnequip(Inventory inventory, Weapon weapon, Logger logger);
    void UnequipAll(Inventory inventory, Logger logger);
}