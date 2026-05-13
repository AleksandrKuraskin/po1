using ConsoleRpg.Model.Core.Map;
using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Systems.Attacking;
using ConsoleRpg.Model.Systems.Logging;
using ConsoleRpg.Model.Systems.Sound;
using ConsoleRpg.Model.Systems.Stats;

namespace ConsoleRpg.Model.Items;

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
            LogManager.Instance.Log($"Inventory full! Cannot pick up {Name}.", LogType.Warning);
        }
        else
        {
            LogManager.Instance.Log($"Added {Name} to inventory.");
        }
        return added;
    }

    public void OnEquip(Player player) {}
    public void OnUnequip(Player player) {}

    public void OnDrop(Map map, int x, int y, IItem item)
    {
        map.GetTile(x, y).AddItem(item);
        LogManager.Instance.Log($"Dropped {Name}.");
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