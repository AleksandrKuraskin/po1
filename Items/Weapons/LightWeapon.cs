using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items.Weapons;

public class LightWeapon(string name, int maxHealth, int damage, IEquipBehavior behavior) : Weapon(behavior)
{
    public override string Name { get; } = name;

    public override StatsManager Stats { get; } = new StatsManager()
        .AddStat(StatType.Health, maxHealth)
        .AddStat(StatType.Strength, damage);

    public override CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        return visitor.VisitLightWeapon(this, player);
    }
}