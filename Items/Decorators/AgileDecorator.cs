using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Items.Decorators;

public class AgileDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _agilityBoost = new FlatModifier(5);

    public override string Name => _item.Name + " (Agile)";

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        player.Stats.AddModifier(StatType.Agility, _agilityBoost);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.Stats.RemoveModifier(StatType.Agility, _agilityBoost);
    }
}