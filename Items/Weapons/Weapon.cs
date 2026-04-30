using System;
using System.Collections.Generic;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Sound;
using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Items.Weapons;

public abstract class Weapon(IEquipBehavior behavior) : IItem
{
    public abstract string Name { get; }
    public virtual char Symbol { get; } = 'w';

    public abstract Loudness Loudness { get; }

    private readonly IEquipBehavior _equipBehavior = behavior;
    private readonly List<(StatType, IStatModifier)> _appliedModifiers = new();
    
    public abstract StatsManager ItemStats { get; }
    public abstract StatsManager GrantedStats { get; }

    public bool TryPickUp(Player player, IItem item)
    {
        var added = player.Inventory.TryAddItem(item);
        if (!added)
        {
            LogManager.Instance.Log($"Inventory full! Cannot pick up {Name}.", LogType.Warning);
        }
        else
        {
            LogManager.Instance.Log($"Added {Name} to inventory.");
        }
        
        return added;
    }

    public virtual void MakeNoise()
    {
        throw new NotImplementedException();
    }

    public IItem? TryEquip(Player player, IItem item, bool leftHand)
    {
        return _equipBehavior.Equip(player, item, leftHand);
    }

    public virtual void OnEquip(Player player)
    {
        foreach (var statType in GrantedStats.GetActiveStatTypes())
        {
            var bonusValue = GrantedStats.GetStat(statType).Value;

            var mod = new FlatModifier(bonusValue);
            player.Stats.AddModifier(statType, mod);
            
            _appliedModifiers.Add((statType, mod));
        }
    }

    public virtual void OnUnequip(Player player)
    {
        foreach (var (statType, mod) in _appliedModifiers)
        {
            player.Stats.RemoveModifier(statType, mod);
        }
        _appliedModifiers.Clear();
    }

    public void OnDrop(Map map, int x, int y, IItem item)
    {
        map.GetTile(x, y).AddItem(item);
        LogManager.Instance.Log($"Dropped {Name}.");
    }

    public abstract CombatStats Accept(IAttackVisitor visitor, Player player);
}