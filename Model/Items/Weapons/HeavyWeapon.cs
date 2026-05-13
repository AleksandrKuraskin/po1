using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Systems.Attacking;
using ConsoleRpg.Model.Systems.Sound;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Items.Weapons;

public class HeavyWeapon(string name, int maxHealth, int damage, IEquipBehavior behavior) : Weapon(behavior)
{
    public override string Name { get; } = name;
    public override Loudness Loudness { get; } = Loudness.Loud;

    public override StatsManager ItemStats { get; } = new StatsManager()
        .AddStat(StatType.Health, maxHealth);
    public override StatsManager GrantedStats { get; } = new StatsManager()
        .AddStat(StatType.Strength, damage);

    public override CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        return visitor.VisitHeavyWeapon(this, player);
    }
}