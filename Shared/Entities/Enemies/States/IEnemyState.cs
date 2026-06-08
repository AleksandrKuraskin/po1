namespace ConsoleRpg.Shared.Entities.Enemies.States;

public interface IEnemyState
{
    IEnemyState HandleSound(IEnemyState newState);
    IEnemyState HandleSight(IEnemyState newState);
    IEnemyState HandleSightLost(IEnemyState defaultState);
    
    IEnemyState ExecuteAction(Enemy enemy, Map.Map map, SpeciesGroup group, IEnemyState defaultState);
}