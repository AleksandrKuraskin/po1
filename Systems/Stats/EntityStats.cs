namespace ConsoleRpg.Systems.Stats;

public class EntityStats(int maxHealth, int strength, int intelligence, int agility, int luck)
{
    public static EntityStats Empty = new(0, 0, 0, 0, 0);
    
    public Stat MaxHealth { get; } = new Stat(maxHealth);
    public Stat Strength { get; } = new Stat(strength);
    public Stat Intelligence { get; } = new Stat(intelligence);
    public Stat Agility { get; } = new Stat(agility);
    public Stat Luck { get; } = new Stat(luck);
}