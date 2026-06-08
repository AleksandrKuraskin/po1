using System;
using ConsoleRpg.Shared.Map;

namespace ConsoleRpg.Shared.Entities.Enemies.States;

public class FleeSoundState(int threatX, int threatY) : IEnemyState
{
    private readonly int _threatX = threatX;
    private readonly int _threatY = threatY;

    public string StateName => "Fleeing Sound";

    public IEnemyState HandleSound(IEnemyState proposedState) => proposedState;
    public IEnemyState HandleSight(IEnemyState proposedState) => proposedState;
    public IEnemyState HandleSightLost(IEnemyState defaultState) => this;

    public IEnemyState ExecuteAction(Enemy enemy, Map.Map map, SpeciesGroup group, IEnemyState defaultState)
    {
        if (!enemy.CanMove) return this;

        var dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
        (int dx, int dy) bestDir = (0, 0);
        int currentDist = Math.Abs(enemy.X - _threatX) + Math.Abs(enemy.Y - _threatY);
        int maxDist = currentDist;

        foreach (var (dx, dy) in dirs)
        {
            int nx = enemy.X + dx;
            int ny = enemy.Y + dy;

            if (nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height)
            {
                var tile = map.GetTile(nx, ny);
                if (!tile.IsWall && tile.Enemy == null)
                {
                    int dist = Math.Abs(nx - _threatX) + Math.Abs(ny - _threatY);
                    if (dist > maxDist)
                    {
                        maxDist = dist;
                        bestDir = (dx, dy);
                    }
                }
            }
        }

        if (bestDir.dx == 0 && bestDir.dy == 0) return defaultState;

        if (map.TryMoveEnemy(enemy, bestDir.dx, bestDir.dy))
        {
            enemy.ResetMoveCooldown();
            enemy.ActedThisTurn = true;
        }
        return this;
    }
}
