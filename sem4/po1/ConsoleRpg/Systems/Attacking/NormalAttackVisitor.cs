using ConsoleRpg.Entities;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Systems.Attacking;

public class NormalAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        var weaponDamage = w.Stats.GetStat(StatType.Strength).Value;
        var playerDamage = p.GetTotalStat(StatType.Strength) + p.GetTotalStat(StatType.Aggression);

        return new CombatStats {
            Attack = weaponDamage + playerDamage,
            Defense = p.GetTotalStat(StatType.Strength) + p.GetTotalStat(StatType.Luck)
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        var weaponDamage = w.Stats.GetStat(StatType.Strength).Value;
        var playerDamage = p.GetTotalStat(StatType.Agility) + p.GetTotalStat(StatType.Luck);

        return new CombatStats {
            Attack = weaponDamage + playerDamage,
            Defense = p.GetTotalStat(StatType.Agility) + p.GetTotalStat(StatType.Luck)
        };
    }

    public CombatStats VisitMagicWeapon(MagicWeapon w, Player p)
    {
        return new CombatStats {
            Attack = 1, 
            Defense = p.GetTotalStat(StatType.Agility) + p.GetTotalStat(StatType.Luck)
        };
    }

    public CombatStats VisitNonWeapon(IItem? item, Player p)
    {
        return new CombatStats
        {
            Attack = 0,
            Defense = p.GetTotalStat(StatType.Agility)
        };
    }
}