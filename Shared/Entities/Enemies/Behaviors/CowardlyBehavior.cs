using ConsoleRpg.Shared.Entities.Enemies.States;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public class CowardlyBehavior : IEnemyBehavior
{
    public IEnemyState GetDefaultState() => new IdleState();
    public IEnemyState? GetSoundReaction(int x, int y) => new FleeSoundState(x, y);
    public IEnemyState? GetSightReaction(Enemy self, List<Player> visiblePlayers)
    {
        if (visiblePlayers.Count == 0) return null;
        return new FleePlayerState(visiblePlayers);
    }
    public void OnAttacked(Enemy self) { }
    public void ApplyDeathReaction(Enemy self)
    {
        self.Stats.AddModifier(StatType.Strength, new PercentModifier(-0.2f));
    }
}