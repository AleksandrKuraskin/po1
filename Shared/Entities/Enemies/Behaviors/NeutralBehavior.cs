using ConsoleRpg.Shared.Entities.Enemies.States;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public class NeutralBehavior : IEnemyBehavior
{
    private bool _attacked = false;
    private bool _healthy;

    public IEnemyState GetDefaultState() => new IdleState();
    
    public IEnemyState? GetSoundReaction(int x, int y)
    {
        if (!_attacked) return null;
        return _healthy ? new ChaseState(x, y, false) : new FleeSoundState(x, y);
    }

    public IEnemyState? GetSightReaction(Enemy self, List<Player> visiblePlayers)
    {
        if (!_attacked || visiblePlayers.Count == 0) return null;
        var player = visiblePlayers[0];
        return _healthy ? new ChaseState(player.X, player.Y, true) : new FleePlayerState(visiblePlayers);
    }

    public void OnAttacked(Enemy self)
    {
        _attacked = true;
        
        var currentHp = self.Stats.GetStat(StatType.Health).BaseValue;
        var maxHp = self.Stats.GetStat(StatType.MaxHealth).BaseValue;
        _healthy = (float)currentHp / maxHp >= 0.5f;
    }

    public void ApplyDeathReaction(Enemy self) { }
}