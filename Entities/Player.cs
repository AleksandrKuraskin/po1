using ConsoleRpg.Core;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities;

public class Player(int startX = 0, int startY = 0) : IEntity
{

    public string Name { get; protected set; } = "Player";
    public char Symbol { get; } = '¶';
    public char GetSymbol() => Symbol;
    
    public int X { get; set; } = startX;
    public int Y { get; set; } = startY;
    
    public EntityStats Stats { get; } = new EntityStats(100, 10, 10, 10, 10);
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();

    public void SetPosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }
}