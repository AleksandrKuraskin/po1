using System;
using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items.Currency;

public abstract class Currency(int value) : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }

    public int Value { get; protected set; } = value;

    protected abstract void AddToWallet(Wallet wallet, Logger logger);

    public bool TryPickUp(Player player, Logger logger)
    {
        AddToWallet(player.Wallet, logger);
        return true;
    }
    
    public IItem? TryEquip(IEquipment equipment, IInventory inventory, bool leftHand, Logger logger)
    {
        logger.Log($"{Name} cannot be equipped", LogType.Error);
        return null;
    }

    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        throw new NotImplementedException();
    }
}