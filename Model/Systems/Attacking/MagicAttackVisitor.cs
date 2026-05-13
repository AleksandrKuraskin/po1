using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Items.Weapons;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Systems.Attacking;

public class MagicAttackVisitor : IAttackVisitor
{
    public CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p)
    {
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        return new CombatStats {
            Attack = 1, 
            Defense = luck
        };
    }
    
    public CombatStats VisitLightWeapon(LightWeapon w, Player p)
    {
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        
        return new CombatStats {
            Attack = 1, 
            Defense = luck
        };
    }

    public CombatStats VisitMagicWeapon(MagicWeapon w, Player p)
    {
        var intelligence = p.Stats.GetStat(StatType.Intelligence).Value;
        
        var playerDamage = intelligence;

        return new CombatStats {
            Attack = playerDamage,
            Defense = intelligence * 2
        };
    }

    public CombatStats VisitNonWeapon(IItem? item, Player p)
    {
        var luck = p.Stats.GetStat(StatType.Luck).Value;
        
        return new CombatStats
        {
            Attack = 0,
            Defense = luck
        };
    }
}