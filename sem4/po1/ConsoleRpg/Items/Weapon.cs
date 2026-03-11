using ConsoleRpg.Core;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items;

public abstract class Weapon : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }
    
    public abstract bool IsTwoHanded { get; protected set; }
    
    public void OnPickUp(Player player, Logger logger)
    {
        var message = !player.Inventory.TryAddItem(this)
            ? $"Inventory full! Cannot pick up {Name}."
            : $"Added {Name} to inventory.";
        
        logger.Log(message);
    }

    public void TryEquip(Player player, Logger logger)
    {
        if (IsTwoHanded) player.Equipment.EquipTwoHanded(this);
        else player.Equipment.EquipRightHand(this);
        
        logger.Log($"Equipped {Name}.");
    }
    
    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        map.GetTile(x, y).AddItem(this);
        logger.Log($"Dropped {Name}.");
    }
}