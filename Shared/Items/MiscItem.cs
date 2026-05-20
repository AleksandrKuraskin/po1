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
            LogManager.Instance.Log($"Inventory full! Cannot pick up {Name}.", entity: player.Name, recipientId: player.Id, type: LogType.Warning);
        }
        else
        {
            LogManager.Instance.Log($"Picked up {Name}.", entity: player.Name, type: LogType.Action);
        }
        return added;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}

    public void OnDrop(Player player, Map.Map map, IItem item)
    {
        map.GetTile(player.X, player.Y).AddItem(item);
        LogManager.Instance.Log($"Dropped {Name}.", entity: player.Name, type: LogType.Action);
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