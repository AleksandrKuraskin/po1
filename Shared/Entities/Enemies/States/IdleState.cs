using ConsoleRpg.Shared.Systems.Graph;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Entities.Enemies.States;

public class IdleState : IEnemyState
{
    private Queue<(int x, int y)> _currentPath = new();
    private readonly Random _rng = new Random();
    
    public IEnemyState HandleSound(IEnemyState newState) => newState;
    public IEnemyState HandleSight(IEnemyState newState) => newState;
    public IEnemyState HandleSightLost(IEnemyState defaultState) => this;

    public IEnemyState ExecuteAction(Enemy enemy, Map.Map map, SpeciesGroup group, IEnemyState defaultState)
    {
        if (!enemy.CanMove) return this;

        if (_currentPath.Count == 0)
        {
            var center = group.GetGroupCenter();
            var offsetX = _rng.Next(-group.MaxRadius, group.MaxRadius + 1);
            var offsetY = _rng.Next(-group.MaxRadius, group.MaxRadius + 1);

            var targetX = Math.Clamp(center.X + offsetX, 1, map.Width - 2);
            var targetY = Math.Clamp(center.Y + offsetY, 1, map.Height - 2);

            _currentPath = Pathfinder.FindPath(map, enemy.X, enemy.Y, targetX, targetY);

            if (_currentPath.Count > 0)
            {
                LogManager.Instance.Log($"[AI] {enemy.Name} is patrolling towards {targetX},{targetY}.");
            }
        }

        if (_currentPath.Count > 0)
        {
            var next = _currentPath.Dequeue();
            var dx = next.x - enemy.X;
            var dy = next.y - enemy.Y;

            if (map.TryMoveEnemy(enemy, dx, dy))
            {
                enemy.ResetMoveCooldown();
                enemy.ActedThisTurn = true;
            }
            else
            {
                _currentPath.Clear();
            }
        }

        return this;
    }
}