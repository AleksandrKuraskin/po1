using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Core;

public interface IEntity : IGameObject
{
    public StatsManager Stats { get; }
    public Inventory Inventory { get; }
    public Equipment Equipment { get; }
    public Wallet Wallet { get; }
    public bool Alive { get; }
    public void TakeDamage(int amount);
}
