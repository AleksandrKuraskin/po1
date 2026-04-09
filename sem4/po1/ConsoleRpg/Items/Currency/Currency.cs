using System;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Currency;

public abstract class Currency(int value) : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }
    
    public StatsManager Stats { get; } = new StatsManager();

    public int Value { get; protected set; } = value;

    protected abstract void AddToWallet(Wallet wallet, Logger logger);

    public bool TryPickUp(Player player, IItem item, Logger logger)
    {
        AddToWallet(player.Wallet, logger);
        return true;
    }
    
    public IItem? TryEquip(Player player, IItem item, bool leftHand, Logger logger)
    {
        logger.Log($"{Name} cannot be equipped", LogType.Error);
        return null;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}
    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        throw new NotImplementedException();
    }

    public CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        throw new NotImplementedException();
    }
}