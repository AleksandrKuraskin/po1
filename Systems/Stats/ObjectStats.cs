namespace ConsoleRpg.Systems.Stats;

public class ObjectStats(int maxHealth, int damage)
{
    public readonly static ObjectStats Empty = new(0, 0);
    
    public Stat MaxHealth { get; } = new Stat(maxHealth);
    public Stat Damage { get; } = new Stat(damage);
}