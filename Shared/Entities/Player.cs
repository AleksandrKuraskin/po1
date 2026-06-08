using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Items.Currency;
using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities;

public class Player : IEntity, ISoundEmitter, ISoundReceiver
{

    private ISoundMediator? _mediator;
    public Guid? GroupId { get; set; }
    public string Name { get; set; }
    public int PlayerNumber { get; set; } = 0;
    public char Symbol => PlayerNumber > 0 ? PlayerNumber.ToString()[0] : '¶';
    
    public Loudness Loudness => Loudness.Soft;

    public StatsManager Stats { get; } = new StatsManager();

    public int X { get; set; }
    public int Y { get; set; }
    public bool Alive => Stats.GetStat(StatType.Health).Value > 0;
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();
    public event Action<int, int>? DirtyMarked;

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
            .AddStat(StatType.Luck, 10)
            .AddStat(StatType.Speed, 10);
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

    public void RemoveMediator()
    {
        _mediator?.RemoveReceiver(this);
        _mediator = null;
    }

    public void OnHeardSound(ISoundEmitter emitter, (int X, int Y) origin, int distance, ISoundEvent sound)
    {
        if (emitter == this) return;
        LogManager.Instance.Log($"You heard {sound.GetFullDescription()} from {origin.X}, {origin.Y} (dist: {distance})", recipientName: Name, type: LogType.Sound);
    }

    public void TakeDamage(int amount)
    {
        var hpStat = Stats.GetStat(StatType.Health);
        hpStat.Decrease(amount);
        if (!Alive) DirtyMarked?.Invoke(X, Y);
    }
    public void SetPosition(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }
public void DropAll(Map.Map map)
{
    var tile = map.GetTile(X, Y);

    // 1. Unequip everything. Standard unequip logic moves to inventory or drops to ground if full.
    if (Equipment.LeftHand != null && Equipment.LeftHand == Equipment.RightHand)
    {
        var dropped = Equipment.EquipTwoHanded(this, null);
        if (dropped != null) tile.AddItem(dropped);
    }
    else
    {
        if (Equipment.LeftHand != null)
        {
            var dropped = Equipment.EquipOneHanded(this, null, true);
            if (dropped != null) tile.AddItem(dropped);
        }
        if (Equipment.RightHand != null)
        {
            var dropped = Equipment.EquipOneHanded(this, null, false);
            if (dropped != null) tile.AddItem(dropped);
        }
    }

    // 2. Drop all items currently in the inventory
    var items = Inventory.GetItems();
    for (var i = 0; i < Inventory.Capacity; i++)
    {
        var item = items[i];
        if (item == null) continue;

        tile.AddItem(item);
        Inventory.RemoveItemAt(i);
    }

    // 3. Drop currency
    if (Wallet.GoldValue > 0) tile.AddItem(new Gold(Wallet.GoldValue));
    if (Wallet.CoinValue > 0) tile.AddItem(new Coin(Wallet.CoinValue));

    Wallet.GoldValue = 0;
    Wallet.CoinValue = 0;

    Equipment.Clear();
}
    public void Die(Map.Map map)
    {
        DropAll(map);
        map.GetTile(X, Y).Players.Remove(this);
        DirtyMarked?.Invoke(X, Y);
    }
}
