using ConsoleRpg.Core;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities.Enemies.Behaviors;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities.Enemies;

public class Enemy(string name, char symbol, int maxHealth, int strength, int armor, IEnemyBehavior behavior, SpeciesGroup group) : IEntity, ISpeciesObserver
{
    private readonly SpeciesGroup _group = group;
    private readonly IEnemyBehavior _behavior = behavior;
    private bool _insideGroup = true;
    
    public string Name { get; protected set; } = name;
    public char Symbol { get; } = symbol;
    
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public StatsManager Stats { get; } = new StatsManager()
        .AddStat(StatType.MaxHealth, maxHealth)
        .AddStat(StatType.Health, maxHealth)
        .AddStat(StatType.Armor, armor)
        .AddStat(StatType.Strength, strength);
    
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();
    
    public void OnMemberDied(Enemy member)
    {
        if (member != this)
        {
            _behavior.ApplyDeathReaction(this);
        }
    }

    public void OnMemberMoved((int X, int Y)newCenter)
    {
        var distance = Math.Abs(X - newCenter.X) + Math.Abs(Y - newCenter.Y);
        _insideGroup = distance <= _group.MaxRadius;
    }
    
    public void TakeTurn(Map map)
    {
        if (!Alive) return;

        var dx = 0;
        var dy = 0;

        if (!_insideGroup)
        {
            var target = _group.GetGroupCenter();
            dx = target.X - X;
            dy = target.Y - Y;

            if (Math.Abs(X - target.X) < Math.Abs(Y - target.Y)) dx = 0;
            else dy = 0;
        }
        else
        {
            var rng = new Random();
            var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (0, 0) };
            var move = dirs[rng.Next(dirs.Length)];
            dx = move.Item1;
            dy = move.Item2;
        }

        if (dx != 0 || dy != 0)
        {
            map.TryMoveEnemy(this, dx, dy);
        }
    }

    public void SetPosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
        _group.NotifyMemberMove(this);
    }

    public void TakeDamage(int amount)
    {
        var hpStat = Stats.GetStat(StatType.Health);
        hpStat.Decrease(amount);
    }
    
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;

    public void Die()
    {
        _group.NotifyMemberDeath(this);
    }
}