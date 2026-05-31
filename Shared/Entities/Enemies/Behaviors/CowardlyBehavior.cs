using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public class CowardlyBehavior : IEnemyBehavior
{
    public void ApplyDeathReaction(Enemy self)
    {
        self.Stats.AddModifier(StatType.Strength, new PercentModifier(-0.2f));
    }
}