using ConsoleRpg.Core;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items;

public class Gold(int value) : Currency(value)
{
    public override char Symbol => 'g';
    public override string Name => Value > 1 ? $"Gold x{Value}" : "Gold";

    protected override void AddToWallet(Wallet wallet, Logger logger)
    {
        wallet.AddGold(Value);
    }
}