using System;
using System.Collections.Generic;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.Items;

namespace ConsoleRpg.Core.Map.Procedures;

public class ItemProcedure(int count, Func<Random, IItem> itemMethod) : IMapProcedure
{
    private readonly int _count = count;
    private readonly Random _rng = new Random();

    public void Apply(MapContext context)
    {
        var freeTiles = new List<Tile>();
        for (var y = 1; y < context.Map.Height - 1; y++)
        for (var x = 1; x < context.Map.Width - 1; x++)
            if (!context.Map.GetTile(x, y).IsWall)
                freeTiles.Add(context.Map.GetTile(x, y));

        for (var i = 0; i < _count && freeTiles.Count > 0; i++)
        {
            var tile = freeTiles[_rng.Next(freeTiles.Count)];
            tile.AddItem(itemMethod.Invoke(_rng));
            freeTiles.Remove(tile);
        }

        if (!context.Itemized && _count > 0)
        {
            context.Itemized = true;
            context.SidebarComponents.Add(new EquipmentComponent());
            context.SidebarComponents.Add(new InventoryComponent());
            context.SidebarComponents.Add(new GroundItemsComponent());
        }
    }
}