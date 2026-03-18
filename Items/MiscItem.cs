using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public class MiscItem(string name, char symbol) : IItem
{
    public string Name { get; } = name;
    public char Symbol { get; } = symbol;
    public ObjectStats Stats { get; } = new ObjectStats(0 ,0); 

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

    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        map.GetTile(x, y).AddItem(this);
        logger.Log($"Dropped {Name}.");
    }

    public IItem? TryEquip(IEquipment equipment, IInventory inventory, bool leftHand, Logger logger)
    {
        return equipment.EquipOneHanded(inventory, this, leftHand, logger);
    }
}