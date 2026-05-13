using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Entities.Enemies.Behaviors;

public class CowardlyBehavior : IEnemyBehavior
{
    public void ApplyDeathReaction(Enemy self)
    {
        self.Stats.AddModifier(StatType.Strength, new PercentModifier(-0.2f));
    }
}