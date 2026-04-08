using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public class MiscItem(string name, char symbol) : IItem
{
    public string Name { get; } = name;
    public char Symbol { get; } = symbol;
    
    public StatsManager Stats { get; } = new StatsManager();

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

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}

    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        map.GetTile(x, y).AddItem(this);
        logger.Log($"Dropped {Name}.");
    }

    public IItem? TryEquip(Player player, bool leftHand, Logger logger)
    {
        return player.Equipment.EquipOneHanded(player, this, leftHand, logger);
    }

    public CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        return visitor.VisitNonWeapon(this, player);
    }
}