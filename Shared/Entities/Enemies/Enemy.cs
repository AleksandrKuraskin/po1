using System;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Entities.Enemies.Behaviors;
using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities.Enemies;

public class Enemy(string name, char symbol, int maxHealth, int strength, int armor, SpeciesGroup group) : IEntity, ISpeciesObserver, ISoundReceiver
{
    private readonly SpeciesGroup _group = group;
    private ISoundMediator? _mediator;
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

    public bool ActedThisTurn { get; set; }
    
    private void Die()
    {
        _group.NotifyMemberDeath(this);
        _group.Detach(this);
        _mediator?.RemoveReceiver(this);
    }
    
    public void OnMemberDied(ISpeciesObserver member)
    {
        if (member == this) return;
        _group.Behavior.ApplyDeathReaction(this);
        LogManager.Instance.Log(
            $"({Name}) alters its stats since group member died."
        );
    }

    public void OnMemberMoved((int X, int Y)newCenter)
    {
        var distance = Math.Abs(X - newCenter.X) + Math.Abs(Y - newCenter.Y);
        _insideGroup = distance <= _group.MaxRadius;
    }
    
    public void TakeTurn(Map map)
    {
        if (!Alive || ActedThisTurn) return;
        var rng = new Random();
        if (rng.Next(100) < 50) return;

        var dx = 0;
        var dy = 0;

        if (!_insideGroup)
        {
            var target = _group.GetGroupCenter();
            
            dx = Math.Sign(target.X - X);
            dy = Math.Sign(target.Y - Y);
            
            if (Math.Abs(target.X - X) < Math.Abs(target.Y - Y)) dx = 0;
            else dy = 0;
        }
        else
        {
            var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
            var move = dirs[rng.Next(dirs.Length)];
            dx = move.Item1;
            dy = move.Item2;
        }

        if (!map.TryMoveEnemy(this, dx, dy))
        {
            if (dx != 0)
            {
                var ddy = rng.Next(2) == 0 ? 1 : -1;
                map.TryMoveEnemy(this, 0, ddy);
            }
            else
            {
                var ddx = rng.Next(2) == 0 ? 1 : -1;
                map.TryMoveEnemy(this, ddx, 0);
            }
        }
    }

    public void SetMediator(ISoundMediator mediator)
    {
        _mediator = mediator;
    }
    public void OnHeardSound(ISoundEmitter emitter, (int X, int Y) origin, int distance, ISoundEvent sound)
    {
        LogManager.Instance.Log(
            $"{Name} heard {sound.GetFullDescription()} from {origin.X}, {origin.Y} (dist: {distance})",
            LogType.Info,
            LogScope.Global,
            null,
            Name
        );
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
        if (!Alive) Die();
    }
    
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;

    public void Spawn(int x, int y)
    {
        X = x;
        Y = y;
        _group.Attach(this);
        _mediator?.AddReceiver(this);
    }
}