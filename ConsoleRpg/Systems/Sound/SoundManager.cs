using System.Collections.Generic;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Systems.Sound;

public class SoundManager(Map map) : ISoundMediator
{
    private readonly Map _map = map;
    private List<ISoundReceiver> _receivers = new();
    
    public void AddReceiver(ISoundReceiver receiver)
    {
        _receivers.Add(receiver);
    }

    public void RemoveReceiver(ISoundReceiver receiver)
    {
        _receivers.Remove(receiver);
    }

    public void EmitSound(ISoundEmitter emitter, (int X, int Y) origin, ISoundEvent sound)
    {
        var radius = (int)emitter.Loudness;
        if (radius <= 0) return;

        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int x, int y, int dist)>();

        queue.Enqueue((origin.X, origin.Y, 0));
        visited.Add(origin);

        while (queue.Count > 0)
        {
            var (cx, cy, dist) = queue.Dequeue();
            
            foreach (var receiver in _receivers)
            {
                if (receiver.X == cx && receiver.Y == cy)
                {
                    receiver.OnHeardSound(emitter, origin, dist, sound);
                }
            }

            if (dist >= radius) continue;

            var directions = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            foreach (var (dx, dy) in directions)
            {
                var nx = cx + dx;
                var ny = cy + dy;

                if (nx >= 0 && nx < _map.Width && ny >= 0 && ny < _map.Height)
                {
                    if (!_map.GetTile(nx, ny).IsWall && visited.Add((nx, ny)))
                    {
                        queue.Enqueue((nx, ny, dist + 1));
                    }
                }
            }
        }
    }
}