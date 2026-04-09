using ConsoleRpg.Entities;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Systems.Attacking;

public interface IAttackVisitor
{
    CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p);
    CombatStats VisitLightWeapon(LightWeapon w, Player p);
    CombatStats VisitMagicWeapon(MagicWeapon w, Player p);
    CombatStats VisitNonWeapon(IItem? item, Player p);
}