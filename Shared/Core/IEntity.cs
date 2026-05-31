using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Core;

public interface IEntity : IGameObject
{
    int X { get; }
    int Y { get; }
    
    public StatsManager Stats { get; }
    public Inventory Inventory { get; }
    public Equipment Equipment { get; }
    public Wallet Wallet { get; }
    public bool Alive { get; }
    public void TakeDamage(int amount);
}
