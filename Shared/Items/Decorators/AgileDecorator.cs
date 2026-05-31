using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Items.Decorators;

public class AgileDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _agilityBoost = new FlatModifier(5);

    public const string Id = "agile";
    
    public override string decoratorId { get; } = Id;

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