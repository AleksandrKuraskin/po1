using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Attacking;

public class MagicAttackVisitor : IAttackVisitor
{
    public string Name => "Magic";

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