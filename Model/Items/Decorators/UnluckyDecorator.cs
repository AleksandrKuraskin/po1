using ConsoleRpg.Model.Entities;
using ConsoleRpg.Model.Systems.Stats;
using ConsoleRpg.Model.Systems.Stats.Modifiers;

namespace ConsoleRpg.Model.Items.Decorators;

public class UnluckyDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _luckPenalty = new FlatModifier(-5);

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