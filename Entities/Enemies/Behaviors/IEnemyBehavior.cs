namespace ConsoleRpg.Entities.Enemies.Behaviors;

public interface IEnemyBehavior
{
    void ApplyDeathReaction(Enemy self);
}