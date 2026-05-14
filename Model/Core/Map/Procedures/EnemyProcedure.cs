using System;
using System.Collections.Generic;
using ConsoleRpg.Model.Entities.Enemies;
using ConsoleRpg.View.Components;
using ConsoleRpg.Model.Systems.Sound;

namespace ConsoleRpg.Model.Core.Map.Procedures;

public class EnemyProcedure(int count, Func<Random, ISoundMediator, Enemy> enemyMethod, ISoundMediator mediator) : IMapProcedure
{
    private readonly int _count = count;
    private readonly Random _rng = new Random();

    public void Apply(MapContext context)
    {
        var freeTiles = new List<Tile>();
        
        for (var y = 1; y < context.Map.Height - 1; y++)
        {
            for (var x = 1; x < context.Map.Width - 1; x++)
            {
                if (!context.Map.GetTile(x, y).IsWall)
                {
                    freeTiles.Add(context.Map.GetTile(x, y));
                }
            }
        }

        for (var i = 0; i < _count && freeTiles.Count > 0; i++)
        {
            var tile = freeTiles[_rng.Next(freeTiles.Count)];
            tile.Enemy = enemyMethod.Invoke(_rng, mediator);
            freeTiles.Remove(tile);
        }

        if (!context.Dangerous && _count > 0)
        {
            context.Dangerous = true;
            context.SidebarComponents.Add(new EnemyComponent());
        }
    }
}