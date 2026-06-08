using ConsoleRpg.Shared.Entities.Enemies.States;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public class AgressiveBehavior : IEnemyBehavior
{
    public IEnemyState GetDefaultState() => new IdleState();
    public IEnemyState? GetSoundReaction(int x, int y) => new ChaseState(x, y, false);
    public IEnemyState? GetSightReaction(Enemy self, List<Player> visiblePlayers)
    {
        if (visiblePlayers.Count == 0) return null;
        var target = visiblePlayers[0];
        return new ChaseState(target.X, target.Y, true);
    }
    public void OnAttacked(Enemy self) { }

    public void ApplyDeathReaction(Enemy self)
    {
        self.Stats.AddModifier(StatType.Strength, new PercentModifier(0.2f));
    }
}