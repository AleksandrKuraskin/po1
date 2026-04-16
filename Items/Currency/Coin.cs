using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Logging;

namespace ConsoleRpg.Items.Currency;

public class Coin(int value) : Currency(value)
{

    public override char Symbol => 'c';
    public override string Name => Value > 1 ? $"Coins x{Value}" : "Coin";

    protected override void AddToWallet(Wallet wallet)
    {
        wallet.AddCoins(Value);
        LogManager.Instance.Log($"Picked x{Value} coins.", LogType.Loot);
    }
}