using ConsoleRpg.Model.Systems;
using ConsoleRpg.Model.Systems.Logging;

namespace ConsoleRpg.Model.Items.Currency;

public class Gold(int value) : Currency(value)
{
    public override char Symbol => 'g';
    public override string Name => Value > 1 ? $"Gold x{Value}" : "Gold";

    protected override void AddToWallet(Wallet wallet)
    {
        wallet.AddGold(Value);
        LogManager.Instance.Log($"Picked x{Value} gold.", LogType.Loot);
    }
}