using System;

using ConsoleRpg.Shared.Maps;
namespace ConsoleRpg.Shared.Maps.Procedures;

public class CorridorsProcedure : IMapProcedure
{
    private readonly Random _rng = new Random();

    public void Apply(MapContext context)
    {
        if (context.Rooms.Count < 2) return;

        for (var i = 0; i < context.Rooms.Count - 1; i++)
        {
            var r1 = context.Rooms[i];
            var r2 = context.Rooms[i + 1];
            CarveDogleg(context.Map, r1.CenterX, r1.CenterY, r2.CenterX, r2.CenterY);
        }
        
        var first = context.Rooms[0];
        var last = context.Rooms[^1];
        CarveDogleg(context.Map, last.CenterX, last.CenterY, first.CenterX, first.CenterY);
    }

    private void CarveDogleg(Map map, int x1, int y1, int x2, int y2)
    {
        if (_rng.Next(2) == 0)
        {
            CarveHorizontal(map, x1, x2, y1);
            CarveVertical(map, y1, y2, x2);
        }
        else
        {
            CarveVertical(map, y1, y2, x1);
            CarveHorizontal(map, x1, x2, y2);
        }
    }

    private void CarveHorizontal(Map map, int x1, int x2, int y)
    {
        for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            if (x > 0 && x < map.Width - 1 && y > 0 && y < map.Height - 1)
                map.GetTile(x, y).IsWall = false;
    }

    private void CarveVertical(Map map, int y1, int y2, int x)
    {
        for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            if (x > 0 && x < map.Width - 1 && y > 0 && y < map.Height - 1)
                map.GetTile(x, y).IsWall = false;
    }
}