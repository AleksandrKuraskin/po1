using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Core.Map;
using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Systems.Attacking;
using ConsoleRpg.Model.Systems.Sound;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Items;

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