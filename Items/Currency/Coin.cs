using System.ComponentModel.DataAnnotations;
using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Systems;

namespace ConsoleRpg.Items.Currency;

public class Coin(int value) : Currency(value)
{

    public override char Symbol => 'c';
    public override string Name => Value > 1 ? $"Coins x{Value}" : "Coin";

    protected override void AddToWallet(Wallet wallet, Logger logger)
    {
        wallet.AddCoins(Value);
        logger.Log($"Picked x{Value} coins.", LogType.Loot);
    }
}