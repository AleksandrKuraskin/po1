using ConsoleRpg.Core;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities.Enemies.Behaviors;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Sound;
using ConsoleRpg.Systems.Sound.SoundEvents;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Entities.Enemies;

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
    
    public void OnMemberDied(Enemy member)
    {
        if (member != this)
        {
            _group.Behavior.ApplyDeathReaction(this);
            LogManager.Instance.Log(
                $"({Name}) alters its stats since group member died."
            );
        }
    }

    public void OnMemberMoved((int X, int Y)newCenter)
    {
        var distance = Math.Abs(X - newCenter.X) + Math.Abs(Y - newCenter.Y);
        _insideGroup = distance <= _group.MaxRadius;
        LogManager.Instance.Log(
            $"({Name}) noticed his group member move."
        );
    }
    
    public void TakeTurn(Map map)
    {
        if (!Alive || ActedThisTurn) return;

        var dx = 0;
        var dy = 0;

        if (!_insideGroup)
        {
            LogManager.Instance.Log(
                $"{Name} moving towards group..."
            );
            var target = _group.GetGroupCenter();
            var diffX = Math.Abs(target.X - X);
            var diffY = Math.Abs(target.Y - Y);

            if (diffX < diffY) dy = 1;
            else dx = 1;
        }
        else
        {
            var rng = new Random();
            var dirs = new[] { (0, 1), (0, -1), (1, 0), (-1, 0), (0, 0) };
            var move = dirs[rng.Next(dirs.Length)];
            dx = move.Item1;
            dy = move.Item2;
            LogManager.Instance.Log(
                $"{Name} moving somewhere..."
            );
        }

        if (dx != 0 || dy != 0)
        {
            LogManager.Instance.Log(
                $"{Name} moving..."
            );
            if (!map.TryMoveEnemy(this, dx, dy))
            {
                LogManager.Instance.Log(
                    $"{Name} can't move..."
                );
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
            $"({Name}) heard ({sound.GetFullDescription()}) from ({origin.X}, {origin.Y}) at distance {distance}."
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

    public void Die()
    {
        _group.NotifyMemberDeath(this);
        _group.Detach(this);
        if (_mediator != null) _mediator.RemoveReceiver(this);
    }
}