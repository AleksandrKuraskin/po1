using System;
using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Items.Currency;

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
        LogManager.Instance.Log($"Picked up {Name} ({Value}).", LogType.Info, LogScope.Global, null, player.Name);
        return true;
    }
    
    public IItem? TryEquip(Player player, IItem item, bool leftHand)
    {
        LogManager.Instance.Log($"{Name} cannot be equipped", LogType.Error, LogScope.Private, player.Name, player.Name);
        return null;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}
    public void OnDrop(Player player, Map map, IItem item)
    {
        throw new NotImplementedException();
    }

    public CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        throw new NotImplementedException();
    }
}