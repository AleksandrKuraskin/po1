using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Items.Decorators;

public class UnluckyDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _luckPenalty = new FlatModifier(-5);
    public const string Id = "unlucky";
    public override string decoratorId { get; } = Id;

    public override string Name => _item.Name + " (Unlucky)";

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        player.Stats.AddModifier(StatType.Luck, _luckPenalty);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.Stats.RemoveModifier(StatType.Luck, _luckPenalty);
    }
}