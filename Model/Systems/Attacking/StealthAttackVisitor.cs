using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Items.Weapons;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Systems.Attacking;

public class StealthAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        var strength = p.Stats.GetStat(StatType.Strength).Value;
        var aggression = p.Stats.GetStat(StatType.Aggression).Value;
        
        var playerDamage = strength + aggression;

        return new CombatStats {
            Attack = playerDamage / 2,
            Defense = strength
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        var strength = p.Stats.GetStat(StatType.Strength).Value;
        var agility = p.Stats.GetStat(StatType.Agility).Value;

        var playerDamage = agility + strength;

        return new CombatStats {
            Attack = playerDamage * 2,
            Defense = agility
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