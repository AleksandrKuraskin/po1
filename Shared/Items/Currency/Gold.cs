using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Items.Currency;

public class Gold(int value) : Currency(value)
{
    public override char Symbol => 'g';
    public override string Name => Value > 1 ? $"Gold x{Value}" : "Gold";

    protected override void AddToWallet(Wallet wallet)
    {
        wallet.AddGold(Value);
    }
}