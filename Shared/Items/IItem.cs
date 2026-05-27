using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Network.Dtos;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Items;

public interface IItem : IGameObject, ISoundSource
{
    public StatsManager ItemStats { get; }
    public StatsManager GrantedStats { get; }

    ItemDto GetState();
    
    bool TryPickUp(Player player, IItem item);
    IItem? TryEquip(Player player, IItem item, bool leftHand);
    
    void OnEquip(Player player);
    void OnUnequip(Player player);
    void OnDrop(Player player, Map.Map map, IItem item);

    CombatStats Accept(IAttackVisitor visitor, Player player);
}