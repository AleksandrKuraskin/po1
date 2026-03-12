using ConsoleRpg.Core;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items;

public abstract class Currency(int value) : IItem
{
    public abstract string Name { get; }
    public abstract char Symbol { get; }

    public int Value { get; protected set; } = value;

    protected abstract void AddToWallet(Wallet wallet, Logger logger);

    public void OnPickUp(Player player, Logger logger)
    {
        AddToWallet(player.Wallet, logger);
        logger.Log($"You picked up {this.GetType().Name}");
    }
    
    public void TryEquip(Player player, Logger logger) => logger.Log($"{this.GetType().Name} cannot be equipped");

    public void OnDrop(Map map, int x, int y, Logger logger)
    {
        throw new NotImplementedException();
    }
}