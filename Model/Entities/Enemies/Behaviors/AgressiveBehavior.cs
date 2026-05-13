using ConsoleRpg.Model.Systems.Stats;
using ConsoleRpg.Model.Systems.Stats.Modifiers;

namespace ConsoleRpg.Model.Entities.Enemies.Behaviors;

public class AgressiveBehavior : IEnemyBehavior
{
    public void ApplyDeathReaction(Enemy self)
    {
        self.Stats.AddModifier(StatType.Strength, new PercentModifier(0.2f));
    }
}