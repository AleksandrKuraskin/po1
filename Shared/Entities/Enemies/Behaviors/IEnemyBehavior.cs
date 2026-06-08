using ConsoleRpg.Shared.Entities.Enemies.States;

namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public interface IEnemyBehavior
{
    IEnemyState GetDefaultState();
    IEnemyState? GetSoundReaction(int x, int y);
    IEnemyState? GetSightReaction(Enemy self, List<Player> visiblePlayers);
    void OnAttacked(Enemy self);
    
    void ApplyDeathReaction(Enemy self);
}