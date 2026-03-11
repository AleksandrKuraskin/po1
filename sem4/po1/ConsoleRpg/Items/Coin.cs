using System.ComponentModel.DataAnnotations;
using ConsoleRpg.Core;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items;

public class Coin(int value) : Currency(value)
{

    public override char Symbol => 'c';
    public override string Name => Value > 1 ? $"Coins x{Value}" : "Coin";

    protected override void AddToWallet(Wallet wallet, Logger logger)
    {
        wallet.AddCoins(Value);
    }
}