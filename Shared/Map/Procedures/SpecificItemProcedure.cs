using System;
using System.Collections.Generic;
using ConsoleRpg.Shared.Items;

using ConsoleRpg.Shared.Maps;
namespace ConsoleRpg.Shared.Maps.Procedures;

public class SpecificItemProcedure(IItem item) : IMapProcedure
{
    private readonly Random _rng = new Random();
    public void Apply(MapContext context)
    {
        var freeTiles = new List<Tile>();
        for (var y = 1; y < context.Map.Height - 1; y++)
        for (var x = 1; x < context.Map.Width - 1; x++)
            if (!context.Map.GetTile(x, y).IsWall)
                freeTiles.Add(context.Map.GetTile(x, y));
        
        var tile = freeTiles[_rng.Next(freeTiles.Count)];
        tile.AddItem(item);
    }
}