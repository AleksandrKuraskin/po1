using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Systems;

public class Wallet
{
    public int CoinValue { get; set; }
    public int GoldValue { get; set; }

    public void AddCoins(int coinValue)
    {
        CoinValue += coinValue;
    }
    
    public void AddGold(int goldValue)
    {
        GoldValue += goldValue;
    }
}