using ConsoleRpg.Shared.Maps;
namespace ConsoleRpg.Shared.Maps.Procedures;

public class CentralHallProcedure(int width, int height) : IMapProcedure
{
    private readonly int _w = width;
    private readonly int _h = height;

    public void Apply(MapContext context)
    {
        var startX = (context.Map.Width / 2) - (_w / 2);
        var startY = (context.Map.Height / 2) - (_h / 2);

        var spawnX = context.Map.Width / 2;
        var spawnY = context.Map.Height / 2;
        
        context.SpawnPoint = (spawnX, spawnY);

        for (var y = startY; y < startY + _h; y++)
        {
            for (var x = startX; x < startX + _w; x++)
            {
                if (x > 0 && x < context.Map.Width - 1 && y > 0 && y < context.Map.Height - 1)
                    context.Map.GetTile(x, y).IsWall = false;
            }
        }
    }
}