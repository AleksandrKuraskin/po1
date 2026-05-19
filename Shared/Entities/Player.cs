using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities;

public class Player : IEntity, ISoundEmitter, ISoundReceiver
{

    private ISoundMediator? _mediator;
    public string Name { get; set; }
    public char Symbol { get; } = '¶';
    
    public Loudness Loudness => Loudness.Soft;

    public StatsManager Stats { get; } = new StatsManager();

    public int X { get; set; }
    public int Y { get; set; }
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();

    public Player(int startX = 0, int startY = 0, string name = "Player")
    {
        Name = name;
        SetPosition(startX, startY);
        
        Stats
            .AddStat(StatType.MaxHealth, 100)
            .AddStat(StatType.Health, 100)
            .AddStat(StatType.Armor, 0)
            .AddStat(StatType.Strength, 10)
            .AddStat(StatType.Aggression, 10)
            .AddStat(StatType.Intelligence, 10)
            .AddStat(StatType.Agility, 10)
            .AddStat(StatType.Luck, 10);
    }

    public void MakeNoise(ISoundEvent sound)
    {
        _mediator?.EmitSound(this, (X, Y), sound);
    }

    public void SetMediator(ISoundMediator mediator)
    {
        _mediator = mediator;
        _mediator?.AddReceiver(this);
    }

    public void OnHeardSound(ISoundEmitter emitter, (int X, int Y) origin, int distance, ISoundEvent sound)
    {
        if (emitter == this) return;
        LogManager.Instance.Log(
            $"You heard {sound.GetFullDescription()} from {origin.X}, {origin.Y} (dist: {distance})",
            LogType.Info,
            LogScope.Private,
            Name
        );
    }

    public void TakeDamage(int amount)
    {
        var hpStat = Stats.GetStat(StatType.Health);
        hpStat.Decrease(amount);
    }
    public void SetPosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }
}