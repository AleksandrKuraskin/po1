using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities.Enemies;

public class Enemy(string name, char symbol, int maxHealth, int strength, int armor) : IEntity
{
    public string Name { get; protected set; } = name;
    public char Symbol { get; } = symbol;
    
    public StatsManager Stats { get; } = new StatsManager()
        .AddStat(StatType.MaxHealth, maxHealth)
        .AddStat(StatType.Health, maxHealth)
        .AddStat(StatType.Armor, armor)
        .AddStat(StatType.Strength, strength);
    
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();

    public void TakeDamage(int amount)
    {
        var hpStat = Stats.GetStat(StatType.Health);
        hpStat.Decrease(amount);
    }
    
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;
}