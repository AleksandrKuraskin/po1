using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Network.Dtos;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Items;

public abstract class ItemDecorator(IItem wrappee) : IItem
{
    protected readonly IItem _item = wrappee;
    
    public abstract string decoratorId { get; }
    
    public virtual string Name => _item.Name;
    public virtual char Symbol => _item.Symbol;
    public IItem InnerItem => _item;
    public virtual Loudness Loudness => _item.Loudness;
    public virtual StatsManager ItemStats => _item.ItemStats;
    public virtual StatsManager GrantedStats => _item.GrantedStats;

    public ItemDto GetState()
    {
        var state = _item.GetState();
        state.Name = Name;
        state.Decorators.Add(decoratorId);
        return state;
    }

    public void OnDrop(Player player, Map.Map map, IItem item)
        => _item.OnDrop(player, map, item);
    
    public bool TryPickUp(Player player, IItem item)
    => _item.TryPickUp(player, item);
    
    public IItem? TryEquip(Player player, IItem item, bool leftHand)
    => _item.TryEquip(player, item, leftHand);

    public virtual void OnEquip(Player player)
    => _item.OnEquip(player);
    
    public virtual void OnUnequip(Player player)
    => _item.OnUnequip(player);

    public CombatStats Accept(IAttackVisitor visitor, Player player) => _item.Accept(visitor, player);
}