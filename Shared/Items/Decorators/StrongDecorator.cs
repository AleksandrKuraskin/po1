using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Items.Decorators;

public class StrongDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _strengthBoost = new FlatModifier(10);

    public override string Name => _item.Name + " (Strong)";

    public override void OnEquip(Player player)
    {
        base.OnEquip(player);
        player.Stats.AddModifier(StatType.Strength, _strengthBoost);
    }

    public override void OnUnequip(Player player)
    {
        base.OnUnequip(player);
        player.Stats.RemoveModifier(StatType.Strength, _strengthBoost);
    }
}