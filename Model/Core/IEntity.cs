using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Core;

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
