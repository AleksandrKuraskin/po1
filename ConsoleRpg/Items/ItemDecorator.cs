using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Sound;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public abstract class ItemDecorator(IItem wrappee) : IItem
{
    protected readonly IItem _item = wrappee;
    
    public virtual string Name => _item.Name;
    public virtual char Symbol => _item.Symbol;
    public virtual Loudness Loudness => _item.Loudness;
    public virtual StatsManager ItemStats => _item.ItemStats;
    public virtual StatsManager GrantedStats => _item.GrantedStats;

    public void OnDrop(Map map, int x, int y, IItem item)
        => _item.OnDrop(map, x, y, item);
    
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