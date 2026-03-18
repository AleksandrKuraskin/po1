using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Core;

public interface IEntity : IGameObject
{
    public EntityStats Stats { get; }
    public Inventory Inventory { get; }
    public Equipment Equipment { get; }
    public Wallet Wallet { get; }
}