using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleRpg.Shared.Maps.Themes;

public class LootTable<T>: IEnumerable<Func<T>>
{
    private readonly List<Func<T>> _pool = new();
    
    public void Add(Func<T> factoryMethod)
    {
        _pool.Add(factoryMethod);
    }

    public T GetRandom(Random rng)
    {
        if (_pool.Count == 0)
            throw new InvalidOperationException("Empty loot table");

        return _pool[rng.Next(_pool.Count)]();
    }

    public Func<T> GetRandomMethod(Random rng)
    {
        return _pool.Count > 0 ? _pool[rng.Next(_pool.Count)] : throw new InvalidOperationException("Empty loot table");
    }
    
    public IEnumerator<Func<T>> GetEnumerator() => _pool.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}