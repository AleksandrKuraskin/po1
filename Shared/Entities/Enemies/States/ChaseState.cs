using ConsoleRpg.Shared.Systems.Combat;
using ConsoleRpg.Shared.Systems.Graph;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities.Enemies.States;

public class ChaseState(int targetX, int targetY, bool playerTarget) : IEnemyState
{
    private readonly int _targetX = targetX;
    private readonly int _targetY = targetY;
    private readonly bool _playerTarget = playerTarget;

    public IEnemyState HandleSound(IEnemyState newState)
    {
        return _playerTarget ? this : newState;
    }

    public IEnemyState HandleSight(IEnemyState newState) => newState;
    
    public IEnemyState HandleSightLost(IEnemyState defaultState)
    {
        return _playerTarget ? defaultState : this;
    }

    public IEnemyState ExecuteAction(Enemy enemy, Map.Map map, SpeciesGroup group, IEnemyState defaultState)
    {
        if (!_playerTarget && enemy.X == _targetX && enemy.Y == _targetY)
        {
            LogManager.Instance.Log($"[AI] {enemy.Name} found sound source but nothing here... Back to idling.");
            return defaultState;
        }

        if (_playerTarget && (Math.Abs(enemy.X - _targetX) + Math.Abs(enemy.Y - _targetY) == 1))
        {
            if (enemy.CanAttack)
            {
                var targetTile = map.GetTile(_targetX, _targetY);
                if (targetTile.Players.Count > 0)
                {
                    var player = targetTile.Players[0];
                    CombatManager.EnemyAttacks(map, enemy, player);
                    enemy.ResetAttackCooldown();
                    enemy.ActedThisTurn = true;
                    return this; 
                }
            }
            return this;
        }
        
        if (enemy.CanMove)
        {
            var path = Pathfinder.FindPath(map, enemy.X, enemy.Y, _targetX, _targetY);
            if (path.Count == 0) return defaultState;

            var nextStep = path.Dequeue();
            if (map.TryMoveEnemy(enemy, nextStep.x - enemy.X, nextStep.y - enemy.Y))
            {
                enemy.ResetMoveCooldown();
                enemy.ActedThisTurn = true;
                LogManager.Instance.Log($"[AI] {enemy.Name} chasing " + (_playerTarget ? "player." : "sound."));
            }
        }

        return this;
    }
}