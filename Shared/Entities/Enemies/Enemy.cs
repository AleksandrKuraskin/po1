using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities.Enemies.States;
using ConsoleRpg.Shared.Items.Currency;
using ConsoleRpg.Shared.Systems;
using ConsoleRpg.Shared.Systems.Graph;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Entities.Enemies;

public class Enemy(string name, char symbol, int maxHealth, int strength, int armor, int speed, SpeciesGroup group) : IEntity, ISpeciesObserver, ISoundReceiver
{
    private readonly SpeciesGroup _group = group;
    private ISoundMediator? _mediator;
    private bool _insideGroup = true;

    private int _moveCooldown = 0;
    private int _attackCooldown = 0;

    public bool CanMove => _moveCooldown <= 0;
    public bool CanAttack => _attackCooldown <= 0;

    public void ResetMoveCooldown()
    {
        var speedStat = Stats.GetStat(StatType.Speed).Value;
        _moveCooldown = Math.Max(1, 600 / Math.Max(1, speedStat));
    }

    public void ResetAttackCooldown()
    {
        _attackCooldown = 60;
    }
    
    public string Name { get; protected set; } = name;
    public char Symbol { get; } = symbol;
    
    public int X { get; private set; }
    public int Y { get; private set; }

    public IEnemyState CurrentState { get; set; } = group.Behavior.GetDefaultState();

    public StatsManager Stats { get; } = new StatsManager()
        .AddStat(StatType.MaxHealth, maxHealth)
        .AddStat(StatType.Health, maxHealth)
        .AddStat(StatType.Armor, armor)
        .AddStat(StatType.Strength, strength)
        .AddStat(StatType.Speed, speed);
    
    public Wallet Wallet { get; } =  new Wallet();
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();

    public bool ActedThisTurn { get; set; }
    public event Action<int, int>? DirtyMarked;

    public void DropAll(Map.Map map)
    {
        // To use standard unequip logic (which handles stats and inventory slots),
        // we use a temporary player context.
        var tempPlayer = new Player(X, Y, Name);
        foreach (var stat in Stats.GetActiveStatTypes())
        {
            tempPlayer.Stats.AddStat(stat, Stats.GetStat(stat).BaseValue);
        }
        
        // Copy inventory and equipment to temp player
        for (int i = 0; i < Inventory.Capacity; i++)
        {
            var item = Inventory.GetItems()[i];
            if (item != null) tempPlayer.Inventory.TryAddItem(item, i);
        }
        
        if (Equipment.LeftHand != null && Equipment.LeftHand == Equipment.RightHand)
            tempPlayer.Equipment.EquipTwoHanded(tempPlayer, Equipment.LeftHand);
        else
        {
            if (Equipment.LeftHand != null) tempPlayer.Equipment.EquipOneHanded(tempPlayer, Equipment.LeftHand, true);
            if (Equipment.RightHand != null) tempPlayer.Equipment.EquipOneHanded(tempPlayer, Equipment.RightHand, false);
        }

        var tile = map.GetTile(X, Y);

        // 1. Unequip hands. Mechanics handle dropping to ground if temp inventory is full.
        if (tempPlayer.Equipment.LeftHand != null && tempPlayer.Equipment.LeftHand == tempPlayer.Equipment.RightHand)
        {
            var dropped = tempPlayer.Equipment.EquipTwoHanded(tempPlayer, null);
            if (dropped != null) tile.AddItem(dropped);
        }
        else
        {
            if (tempPlayer.Equipment.LeftHand != null)
            {
                var dropped = tempPlayer.Equipment.EquipOneHanded(tempPlayer, null, true);
                if (dropped != null) tile.AddItem(dropped);
            }
            if (tempPlayer.Equipment.RightHand != null)
            {
                var dropped = tempPlayer.Equipment.EquipOneHanded(tempPlayer, null, false);
                if (dropped != null) tile.AddItem(dropped);
            }
        }

        // 2. Drop all items currently in the temp inventory
        var items = tempPlayer.Inventory.GetItems();
        for (var i = 0; i < tempPlayer.Inventory.Capacity; i++)
        {
            var item = items[i];
            if (item == null) continue;
            tile.AddItem(item);
            tempPlayer.Inventory.RemoveItemAt(i);
        }

        if (Wallet.GoldValue > 0) tile.AddItem(new Gold(Wallet.GoldValue));
        if (Wallet.CoinValue > 0) tile.AddItem(new Coin(Wallet.CoinValue));

        Wallet.GoldValue = 0;
        Wallet.CoinValue = 0;
        
        // Clear original entity state
        Inventory.Clear();
        Equipment.Clear();
    }

    public void Die(Map.Map map)
    {
        DropAll(map);
        map.GetTile(X, Y).Enemy = null;
        DirtyMarked?.Invoke(X, Y);
        _group.NotifyMemberDeath(this);
        _group.Detach(this);
        _mediator?.RemoveReceiver(this);
    }
    
    public void OnMemberDied(ISpeciesObserver member)
    {
        if (member == this) return;
        _group.Behavior.ApplyDeathReaction(this);
        DirtyMarked?.Invoke(X, Y);
        LogManager.Instance.Log($"({Name}) alters its stats since group member died.", type: LogType.Action);
    }

    public void OnMemberMoved((int X, int Y)newCenter)
    {
        var distance = Math.Abs(X - newCenter.X) + Math.Abs(Y - newCenter.Y);
        _insideGroup = distance <= _group.MaxRadius;
    }
    
    public void TakeTurn(Map.Map map)
    {
        if (!Alive) return;

        if (_moveCooldown > 0) _moveCooldown--;
        if (_attackCooldown > 0) _attackCooldown--;

        if (ActedThisTurn) return;

        var defaultState = _group.Behavior.GetDefaultState();
        
        var visiblePlayers = Pathfinder.GetVisiblePlayers(map, X, Y, 8);

        if (visiblePlayers.Count > 0)
        {
            var reaction = _group.Behavior.GetSightReaction(this, visiblePlayers);
            if (reaction != null) CurrentState = CurrentState.HandleSight(reaction);
        }
        else
        {
            CurrentState = CurrentState.HandleSightLost(defaultState);
        }
        
        CurrentState = CurrentState.ExecuteAction(this, map, _group, defaultState);
    }

    public void SetMediator(ISoundMediator mediator)
    {
        _mediator = mediator;
    }
    public void OnHeardSound(ISoundEmitter emitter, (int X, int Y) origin, int distance, ISoundEvent sound)
    {
        var reaction = _group.Behavior.GetSoundReaction(origin.X, origin.Y);
        if (reaction != null)
        {
            CurrentState = CurrentState.HandleSound(reaction);
        }
        LogManager.Instance.Log($"{Name} heard {sound.GetFullDescription()} from {origin.X}, {origin.Y} (dist: {distance})", type: LogType.Sound);
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
        _group.Behavior.OnAttacked(this);
        DirtyMarked?.Invoke(X, Y);
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
