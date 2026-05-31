namespace ConsoleRpg.Shared.Items.Decorators;

public static class DecoratorRegistry
{
    const int MaxDecoratorCount = 3;

    private static readonly List<string> _randomPool = new();
    private static readonly Dictionary<string, Func<IItem, IItem>> _decorators = new();
    
    static DecoratorRegistry()
    {
        Register(StrongDecorator.Id, item => new StrongDecorator(item));
        Register(UnluckyDecorator.Id, item => new UnluckyDecorator(item));
        Register(AgileDecorator.Id, item => new AgileDecorator(item));
        
        Register(GodlyDecorator.Id, item => new GodlyDecorator(item), false);
    }
    
    private static void Register(string id, Func<IItem, IItem> factory, bool randomPool = true)
    {
        _decorators[id] = factory;
        if (randomPool)
        {
            _randomPool.Add(id);
        }
    }
    
    public static bool TryGetDecorator(string id, out Func<IItem, IItem>? decorator)
    {
        return _decorators.TryGetValue(id, out decorator);
    }
    
    public static IItem ApplyRandomDecorators(IItem baseItem, Random rng, double chanceToEnchant = 0.3)
    {
        if (rng.NextDouble() < 0.02) return new GodlyDecorator(baseItem);

        var currentItem = baseItem;
        var decoratorCount = rng.Next(0, MaxDecoratorCount);

        while (rng.NextDouble() < chanceToEnchant && decoratorCount-- > 0)
        {
            var key = _randomPool[rng.Next(_randomPool.Count)];
            currentItem = _decorators[key].Invoke(currentItem);
        }

        return currentItem;
    }
}