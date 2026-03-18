using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities;

public class Enemy : IEntity
{
    public string Name { get; protected set; } = "Enemy";
    public char Symbol { get; } = 'E';
    
    public EntityStats Stats { get; } = EntityStats.Empty;
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();
}