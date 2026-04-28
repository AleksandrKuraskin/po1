using ConsoleRpg.Core;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Attacking;
using ConsoleRpg.Systems.Sound;
using ConsoleRpg.Systems.Stats;

namespace ConsoleRpg.Items;

public interface IItem : IGameObject, ISoundSource
{
    public StatsManager ItemStats { get; }
    public StatsManager GrantedStats { get; }
    bool TryPickUp(Player player, IItem item);
    IItem? TryEquip(Player player, IItem item, bool leftHand);
    
    void OnEquip(Player player);
    void OnUnequip(Player player);
    void OnDrop(Map map, int x, int y, IItem item);

    CombatStats Accept(IAttackVisitor visitor, Player player);
}