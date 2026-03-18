using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items.Currency;

public class Gold(int value) : Currency(value)
{
    public override char Symbol => 'g';
    public override string Name => Value > 1 ? $"Gold x{Value}" : "Gold";

    protected override void AddToWallet(Wallet wallet, Logger logger)
    {
        wallet.AddGold(Value);
        logger.Log($"Picked x{Value} gold.", LogType.Loot);
    }
}