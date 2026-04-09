using ConsoleRpg.Entities;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Systems.Attacking;

public class StealthAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        var weaponDamage = w.Stats.GetStat(StatType.Strength).Value;
        var playerDamage = p.GetTotalStat(StatType.Strength) + p.GetTotalStat(StatType.Aggression);

        return new CombatStats {
            Attack = (weaponDamage + playerDamage) / 2,
            Defense = p.GetTotalStat(StatType.Strength)
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        var weaponDamage = w.Stats.GetStat(StatType.Strength).Value;
        var playerDamage = p.GetTotalStat(StatType.Agility) + p.GetTotalStat(StatType.Luck);

        return new CombatStats {
            Attack = (weaponDamage + playerDamage) * 2,
            Defense = p.GetTotalStat(StatType.Agility)
        };
    }

    public CombatStats VisitMagicWeapon(MagicWeapon w, Player p)
    {
        return new CombatStats {
            Attack = 1, 
            Defense = 0
        };
    }

    public CombatStats VisitNonWeapon(IItem? item, Player p)
    {
        return new CombatStats
        {
            Attack = 0,
            Defense = 0
        };
    }
}