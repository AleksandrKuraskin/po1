using ConsoleRpg.Entities;
using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Items.Decorators;

public class StrongDecorator(IItem item) : ItemDecorator(item)
{
    private readonly IStatModifier _strengthBoost = new FlatModifier(10);

    public override string Name => _item.Name + " (Agile)";

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