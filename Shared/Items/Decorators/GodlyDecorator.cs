using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Stats;
using ConsoleRpg.Shared.Systems.Stats.Modifiers;

namespace ConsoleRpg.Shared.Items.Decorators;

public class GodlyDecorator : ItemDecorator
{
    private readonly Dictionary<StatType, IStatModifier> _playerModifiers;

    private readonly List<IStatModifier> _itemDamageModifiers;
    public const string Id = "godly";
    public override string decoratorId { get; } = Id;
    public override string Name => base.Name + " (Godly)";

    public GodlyDecorator(IItem innerItem) : base(innerItem)
    {
        _playerModifiers = new Dictionary<StatType, IStatModifier>
        {
            { StatType.MaxHealth, new FlatModifier(500) },
            { StatType.Armor, new FlatModifier(100) },
            { StatType.Strength, new FlatModifier(50) },
            { StatType.Agility, new FlatModifier(50) },
            { StatType.Intelligence, new FlatModifier(50) },
            { StatType.Luck, new FlatModifier(50) }
        };
        
        _itemDamageModifiers = new List<IStatModifier>
        {
            new FlatModifier(100),
            new PercentModifier(1.5f)
        };
        
        foreach (var mod in _itemDamageModifiers)
        {
            GrantedStats.AddModifier(StatType.Strength, mod);
            GrantedStats.AddModifier(StatType.Intelligence, mod);
        }
    }
    public override void OnEquip(Player p)
    {
        base.OnEquip(p);
        foreach (var mod in _playerModifiers)
        {
            p.Stats.AddModifier(mod.Key, mod.Value);
        }
        
        p.Stats.GetStat(StatType.Health).Increase(500);
        Console.WriteLine("Modifying player stats!");
    }

    public override void OnUnequip(Player p)
    {
        foreach (var mod in _playerModifiers)
        {
            p.Stats.RemoveModifier(mod.Key, mod.Value);
        }
        
        var currentHp = p.Stats.GetStat(StatType.Health);
        var maxHp = p.Stats.GetStat(StatType.MaxHealth).Value;
        if (currentHp.Value > maxHp)
        {
            currentHp.SetBaseValue(maxHp);
        }
        
        base.OnUnequip(p);
    }
}