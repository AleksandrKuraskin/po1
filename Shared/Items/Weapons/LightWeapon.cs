using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Items.Weapons;

public class LightWeapon(string name, int maxHealth, int damage, IEquipBehavior behavior) : Weapon(behavior)
{
    public override string Name { get; } = name;
    public override Loudness Loudness { get; } = Loudness.Soft;

    public override StatsManager ItemStats { get; } = new StatsManager()
        .AddStat(StatType.Health, maxHealth);
    public override StatsManager GrantedStats { get; } = new StatsManager()
        .AddStat(StatType.Strength, damage);

    public override CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        return visitor.VisitLightWeapon(this, player);
    }
}