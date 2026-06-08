using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleRpg.Shared.Map;

namespace ConsoleRpg.Shared.Entities.Enemies.States;

public class FleePlayerState(List<Player> threats) : IEnemyState
{
    private readonly List<Player> _threats = threats;

    public string StateName => "Fleeing Player";

    public IEnemyState HandleSound(IEnemyState proposedState) => this;
    public IEnemyState HandleSight(IEnemyState proposedState) => proposedState;
    public IEnemyState HandleSightLost(IEnemyState defaultState) => defaultState;

    public IEnemyState ExecuteAction(Enemy enemy, Map.Map map, SpeciesGroup group, IEnemyState defaultState)
    {
        if (!enemy.CanMove) return this;

        var allDirs = new List<(int dx, int dy)> { (0, -1), (0, 1), (-1, 0), (1, 0) };
        var blockedDirs = new HashSet<(int dx, int dy)>();

        foreach (var threat in _threats)
        {
            int dx = Math.Sign(threat.X - enemy.X);
            int dy = Math.Sign(threat.Y - enemy.Y);
            
            if (dx != 0) blockedDirs.Add((dx, 0));
            if (dy != 0) blockedDirs.Add((0, dy));
        }

        var availableDirs = allDirs.Where(d => !blockedDirs.Contains(d)).ToList();

        if (availableDirs.Count == 0)
        {
            // Otoczony z każdej strony lub zablokowany wektorami zagrożeń - brak ruchu, wraca do default
            return defaultState;
        }

        // Filtrujemy pozostałe kierunki przez fizykę mapy (ściany/potwory)
        var walkableDirs = new List<(int dx, int dy)>();
        foreach (var dir in availableDirs)
        {
            int nx = enemy.X + dir.dx;
            int ny = enemy.Y + dir.dy;

            if (nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height)
            {
                var tile = map.GetTile(nx, ny);
                if (!tile.IsWall && tile.Enemy == null)
                {
                    walkableDirs.Add(dir);
                }
            }
        }

        if (walkableDirs.Count == 0)
        {
            return defaultState;
        }

        var rng = new Random();
        var move = walkableDirs[rng.Next(walkableDirs.Count)];

        if (map.TryMoveEnemy(enemy, move.dx, move.dy))
        {
            enemy.ResetMoveCooldown();
            enemy.ActedThisTurn = true;
        }

        return this;
    }
}
