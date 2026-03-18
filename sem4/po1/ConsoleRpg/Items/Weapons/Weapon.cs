using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Weapons;

public abstract class Weapon : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }
    
    public abstract WeaponStats Stats { get; set; }
    public bool TryPickUp(Player player, Logger logger)
    {
        var added = player.Inventory.TryAddItem(this);
        if (!added)
        {
            logger.Log($"Inventory full! Cannot pick up {Name}.", LogType.Warning);
        }
        else
        {
            logger.Log($"Added {Name} to inventory.");
        }
        
        return added;
    }
    
    public abstract IItem? TryEquip(IEquipment equipment, IInventory inventory, bool leftHand, Logger logger);
    
    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        map.GetTile(x, y).AddItem(this);
        logger.Log($"Dropped {Name}.");
    }
}