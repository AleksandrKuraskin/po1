using System;
using System.Collections.Generic;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Map;

namespace ConsoleRpg.Shared.Systems.Graph;

public static class Pathfinder
{
    public static List<Player> GetVisiblePlayers(Map.Map map, int startX, int startY, int range)
    {
        var visiblePlayers = new List<Player>();
        var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

        foreach (var (dx, dy) in directions)
        {
            for (int step = 1; step <= range; step++)
            {
                int cx = startX + (dx * step);
                int cy = startY + (dy * step);

                if (cx < 0 || cx >= map.Width || cy < 0 || cy >= map.Height) break;

                var tile = map.GetTile(cx, cy);
                if (tile.IsWall) break;

                if (tile.Players.Count > 0)
                {
                    visiblePlayers.AddRange(tile.Players);
                    break; 
                }
            }
        }
        return visiblePlayers;
    }

    public static Queue<(int x, int y)> FindPath(Map.Map map, int startX, int startY, int targetX, int targetY)
    {
        var queue = new Queue<(int x, int y)>();
        var parentMap = new Dictionary<(int x, int y), (int x, int y)>();
        var visited = new HashSet<(int x, int y)>();

        queue.Enqueue((startX, startY));
        visited.Add((startX, startY));

        bool found = false;

        while (queue.Count > 0)
        {
            var curr = queue.Dequeue();

            if (curr.x == targetX && curr.y == targetY)
            {
                found = true;
                break;
            }

            var dirs = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };
            foreach (var (dx, dy) in dirs)
            {
                int nx = curr.x + dx;
                int ny = curr.y + dy;

                if (nx >= 0 && nx < map.Width && ny >= 0 && ny < map.Height)
                {
                    bool isTarget = (nx == targetX && ny == targetY);
                    
                    if (!visited.Contains((nx, ny)) && (!map.GetTile(nx, ny).IsWall || isTarget))
                    {
                        visited.Add((nx, ny));
                        parentMap[(nx, ny)] = curr;
                        queue.Enqueue((nx, ny));
                    }
                }
            }
        }

        var path = new Queue<(int x, int y)>();
        if (!found) return path;

        var pathStack = new Stack<(int x, int y)>();
        var step = (targetX, targetY);

        while (step != (startX, startY))
        {
            pathStack.Push(step);
            step = parentMap[step];
        }

        while (pathStack.Count > 0) path.Enqueue(pathStack.Pop());
        return path;
    }
}
