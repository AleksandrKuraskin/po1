using ConsoleRpg.Shared.Systems;

namespace ConsoleRpg.Shared.Items.Currency;

public class Coin(int value) : Currency(value)
{

    public override char Symbol => 'c';
    public override string Name => Value > 1 ? $"Coins x{Value}" : "Coin";

    protected override void AddToWallet(Wallet wallet)
    {
        wallet.AddCoins(Value);
    }
}