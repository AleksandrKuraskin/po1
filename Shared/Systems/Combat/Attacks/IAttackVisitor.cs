using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Attacking;

public interface IAttackVisitor
{
    string Name { get; }
    CombatStats VisitHeavyWeapon(HeavyWeapon w, Player p);
    CombatStats VisitLightWeapon(LightWeapon w, Player p);
    CombatStats VisitMagicWeapon(MagicWeapon w, Player p);
    CombatStats VisitNonWeapon(IItem? item, Player p);
}