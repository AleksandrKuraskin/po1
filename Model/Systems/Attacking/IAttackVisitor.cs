using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Items.Weapons;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Systems.Attacking;

public interface IAttackVisitor
{
    CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p);
    CombatStats VisitLightWeapon(LightWeapon w, Player p);
    CombatStats VisitMagicWeapon(MagicWeapon w, Player p);
    CombatStats VisitNonWeapon(IItem? item, Player p);
}