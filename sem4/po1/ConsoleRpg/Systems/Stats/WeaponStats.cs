namespace ConsoleRpg.Systems.Stats;

public class WeaponStats(int maxHealth, int damage)
{
    public static WeaponStats Empty = new (0, 0);
    
    public int MaxHealth { get; } = maxHealth;
    public Stat CurrentHealth { get; set; } = new Stat(maxHealth);
    public Stat Damage { get; } = new Stat(damage);
    
    public bool IsBroken => CurrentHealth.Value == 0;
    
    public void TakeDamage(int damage)
    {
        CurrentHealth.Value -= damage;
        if (CurrentHealth.Value < 0) CurrentHealth.Value = 0;
    }
    
}