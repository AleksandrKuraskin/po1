namespace ConsoleRpg.Shared.Entities.Enemies.Behaviors;

public interface IEnemyBehavior
{
    void ApplyDeathReaction(Enemy self);
}