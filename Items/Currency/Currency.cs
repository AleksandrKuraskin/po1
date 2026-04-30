using System;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Sound;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Currency;

public abstract class Currency(int value) : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }
    public virtual Loudness Loudness { get; } = Loudness.None;

    public StatsManager ItemStats { get; } = new StatsManager();
    public StatsManager GrantedStats { get; } = new StatsManager();

    public int Value { get; protected set; } = value;

    protected abstract void AddToWallet(Wallet wallet);

    public bool TryPickUp(Player player, IItem item)
    {
        AddToWallet(player.Wallet);
        return true;
    }
    
    public IItem? TryEquip(Player player, IItem item, bool leftHand)
    {
        LogManager.Instance.Log($"{Name} cannot be equipped", LogType.Error);
        return null;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}
    public void OnDrop(Map map, int x, int y, IItem item)
    {
        throw new NotImplementedException();
    }

    public CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        throw new NotImplementedException();
    }
}