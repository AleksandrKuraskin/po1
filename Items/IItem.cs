using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public interface IItem : IGameObject
{
    public StatsManager Stats { get; }
    bool TryPickUp(Player player, Logger logger);
    IItem? TryEquip(Player player, bool leftHand, Logger logger);
    
    void OnEquip(Player player);
    void OnUnequip(Player player);
    void OnDrop(Map map, int x, int y, Logger logger);

    CombatStats Accept(IAttackVisitor visitor, Player player);
}