using ConsoleRpg.Entities;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Systems.Attacking;

public class MagicAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        return new CombatStats {
            Attack = 1, 
            Defense = p.GetTotalStat(StatType.Luck)
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        return new CombatStats {
            Attack = 1, 
            Defense = p.GetTotalStat(StatType.Luck)
        };
    }

    public CombatStats VisitMagicWeapon(MagicWeapon w, Player p)
    {
        var weaponDamage = w.Stats.GetStat(StatType.Strength).Value;
        var playerDamage = p.GetTotalStat(StatType.Intelligence);

        return new CombatStats {
            Attack = weaponDamage + playerDamage,
            Defense = p.GetTotalStat(StatType.Intelligence) * 2
        };
    }

    public CombatStats VisitNonWeapon(IItem? item, Player p)
    {
        return new CombatStats
        {
            Attack = 0,
            Defense = p.GetTotalStat(StatType.Luck)
        };
    }
}