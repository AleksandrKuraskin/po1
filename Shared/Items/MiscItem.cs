using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Attacking;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Items;

public class MiscItem(string name, char symbol) : IItem
{
    public string Name { get; } = name;
    public char Symbol { get; } = symbol;
    public Loudness Loudness { get; } = Loudness.Soft;

    public StatsManager ItemStats { get; } = new StatsManager();
    public StatsManager GrantedStats { get; } = new StatsManager();

    public bool TryPickUp(Player player, IItem item)
    {
        var added = player.Inventory.TryAddItem(item);
        if (!added)
        {
            LogManager.Instance.Log($"Inventory full! Cannot pick up {Name}.", LogType.Warning, LogScope.Private, player.Name, player.Name);
        }
        else
        {
            LogManager.Instance.Log($"Picked up {Name}.", LogType.Info, LogScope.Global, null, player.Name);
        }
        return added;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}

    public void OnDrop(Player player, Map map, IItem item)
    {
        map.GetTile(player.X, player.Y).AddItem(item);
        LogManager.Instance.Log($"Dropped {Name}.", LogType.Info, LogScope.Global, null, player.Name);
    }

    public IItem? TryEquip(Player player, IItem item, bool leftHand)
    {
        return player.Equipment.EquipOneHanded(player, item, leftHand);
    }

    public CombatStats Accept(IAttackVisitor visitor, Player player)
    {
        return visitor.VisitNonWeapon(this, player);
    }
}