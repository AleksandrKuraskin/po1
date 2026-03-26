using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public interface IItem : IGameObject
{
    
    ObjectStats Stats => ObjectStats.Empty;
    
    bool TryPickUp(Player player, Logger logger);
    IItem? TryEquip(IEquipment equipment, IInventory inventory, bool leftHand, Logger logger);
    void OnDrop(Map map, int x, int y, Logger logger);
}