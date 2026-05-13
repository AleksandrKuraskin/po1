namespace ConsoleRpg.Model.Core.Map.Procedures;

public class FillMapProcedure : IMapProcedure
{
    public void Apply(MapContext context)
    {
        for (var y = 0; y < context.Map.Height; y++)
        for (var x = 0; x < context.Map.Width; x++)
            context.Map.GetTile(x, y).IsWall = true;
    }
}