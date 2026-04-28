using ConsoleRpg.Entities;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Systems.Attacking;

public class NormalAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        var strength = p.Stats.GetStat(StatType.Strength).Value;
        var aggression = p.Stats.GetStat(StatType.Aggression).Value;
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        
        var playerDamage = strength + aggression;

        return new CombatStats {
            Attack = playerDamage,
            Defense = strength + luck
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        var agility = p.Stats.GetStat(StatType.Agility).Value;
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        
        var playerDamage = agility + luck;

        return new CombatStats {
            Attack = playerDamage,
            Defense = agility + luck
        };
    }

    public CombatStats VisitMagicWeapon(MagicWeapon w, Player p)
    {
        var agility = p.Stats.GetStat(StatType.Agility).Value;
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        
        return new CombatStats {
            Attack = 1, 
            Defense = agility + luck
        };
    }

    public CombatStats VisitNonWeapon(IItem? item, Player p)
    {
        var agility = p.Stats.GetStat(StatType.Agility).Value;
        
        return new CombatStats
        {
            Attack = 0,
            Defense = agility
        };
    }
}