using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.Systems.Sound;

namespace ConsoleRpg.Core.Map.Procedures;

public class EnemyPackProcedure(int packSize, Func<Random, ISoundMediator, IEnumerable<Enemy>> packMethod, ISoundMediator mediator) : IMapProcedure
{
    private readonly int _packSize = packSize;
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
        
        for (var i = 0; i < _packSize; i++)
        {
            var pack = packMethod.Invoke(_rng, mediator);
            foreach (var enemy in pack)
            {
                if (freeTiles.Count == 0) break;
                var tile = freeTiles[_rng.Next(freeTiles.Count)];
                
                tile.Enemy = enemy;
                enemy.Spawn(tile.X, tile.Y);
                freeTiles.Remove(tile);
            }
        }

        if (!context.Dangerous && _packSize > 0)
        {
            context.Dangerous = true;
            context.SidebarComponents.Add(new EnemyComponent());
        }
    }
}