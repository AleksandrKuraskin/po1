namespace ConsoleRpg.Core.Map.Procedures;

public class EmptyMapProcedure : IMapProcedure
{
    public void Apply(MapContext context)
    {
        for (var y = 0; y < context.Map.Height; y++)
        for (var x = 0; x < context.Map.Width; x++)
            context.Map.GetTile(x, y).IsWall = false;
    }
}