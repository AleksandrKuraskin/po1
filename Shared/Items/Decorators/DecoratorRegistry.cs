using System;
using System.Collections.Generic;

namespace ConsoleRpg.Shared.Items.Decorators;

public class DecoratorRegistry
{
    const int MaxDecoratorCount = 3;
    
    private static readonly List<Func<IItem, IItem>> _decoratorPool = 
    [
        item => new StrongDecorator(item),
        item => new UnluckyDecorator(item),
        item => new AgileDecorator(item),
    ];
    
    public static IItem ApplyRandomDecorators(IItem baseItem, Random rng, double chanceToEnchant = 0.3)
    {
        if (rng.NextDouble() < 0.02) return new GodlyDecorator(baseItem);

        var currentItem = baseItem;
        var decoratorCount = rng.Next(0, MaxDecoratorCount);

        while (rng.NextDouble() < chanceToEnchant && decoratorCount-- > 0)
        {
            var decorator = _decoratorPool[rng.Next(_decoratorPool.Count)];
            currentItem = decorator(currentItem);
        }

        return currentItem;
    }
}