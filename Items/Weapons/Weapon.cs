using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Weapons;

public abstract class Weapon(IEquipBehavior behavior) : IItem
{
    public abstract string Name { get; }
    public virtual char Symbol { get; } = 'w';
    
    private readonly IEquipBehavior _equipBehavior = behavior;
    
    public abstract StatsManager Stats { get; }
    
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

    public IItem? TryEquip(Player player, bool leftHand, Logger logger)
    {
        return _equipBehavior.Equip(player, this, leftHand, logger);
    }

    public virtual void OnEquip(Player player) {}
    public virtual void OnUnequip(Player player) {}

    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        map.GetTile(x, y).AddItem(this);
        logger.Log($"Dropped {Name}.");
    }

    public abstract CombatStats Accept(IAttackVisitor visitor, Player player);
}