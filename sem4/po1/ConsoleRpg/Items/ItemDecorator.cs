using ConsoleRpg.Core.Logger;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public abstract class ItemDecorator(IItem item) : IItem
{
    protected readonly IItem _item = item;
    
    public virtual string Name => _item.Name;
    public virtual char Symbol => _item.Symbol;
    public virtual StatsManager ItemStats => _item.ItemStats;
    public virtual StatsManager GrantedStats => _item.GrantedStats;

    public void OnDrop(Map map, int x, int y, IItem item, Logger logger)
        => _item.OnDrop(map, x, y, item, logger);
    
    public bool TryPickUp(Player player, IItem item, Logger logger)
    => _item.TryPickUp(player, item, logger);
    
    public IItem? TryEquip(Player player, IItem item, bool leftHand, Logger logger)
    => _item.TryEquip(player, item, leftHand, logger);

    public virtual void OnEquip(Player player)
    => _item.OnEquip(player);
    
    public virtual void OnUnequip(Player player)
    => _item.OnUnequip(player);

    public CombatStats Accept(IAttackVisitor visitor, Player player) => _item.Accept(visitor, player);
}