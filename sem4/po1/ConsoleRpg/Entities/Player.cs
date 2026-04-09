using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities;

public class Player : IEntity
{

    public string Name { get; protected set; } = "Player";
    public char Symbol { get; } = '¶';

    public StatsManager Stats { get; } = new StatsManager();

    public int X { get; private set; }
    public int Y { get; private set; }
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();

    public Player(int startX = 0, int startY = 0)
    {
        
        SetPosition(startX, startY);
        
        Stats
            .AddStat(StatType.MaxHealth, 100)
            .AddStat(StatType.Health, 100)
            .AddStat(StatType.Armor, 0)
            .AddStat(StatType.Strength, 10)
            .AddStat(StatType.Aggression, 10)
            .AddStat(StatType.Intelligence, 10)
            .AddStat(StatType.Agility, 10)
            .AddStat(StatType.Luck, 10);
    }
    
    public int GetTotalStat(StatType type)
    {
        return Stats.GetStat(type).Value;
    }

    public void TakeDamage(int amount)
    {
        var hpStat = Stats.GetStat(StatType.Health);
        hpStat.Decrease(amount);
    }
    public void SetPosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }
}