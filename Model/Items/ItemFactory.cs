using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using ConsoleRpg.Model.Items.Currency;
using ConsoleRpg.Model.Items.Decorators;
using ConsoleRpg.Model.Items.Weapons;
using ConsoleRpg.Model.Systems.Stats;
using ConsoleRpg.Model.Systems.Stats.Modifiers;
using Spectre.Console;

namespace ConsoleRpg.Model.Items;

public class ItemFactory
{
    private static readonly Dictionary<string, Func<IItem>> _blueprints = new();

    private static readonly List<string> _weaponIds = new();
    private static readonly List<string> _miscIds = new();
    private static readonly List<string> _currencyIds = new();
    
    public static void Initialize()
    {
        void Add(string id, Func<IItem> factoryMethod, List<string> category)
        {
            _blueprints[id] = factoryMethod;
            category.Add(id);
        }
        Add("rusty_sword", () => new LightWeapon("Rusty Sword", 10, 2, new EquipOneHanded()), _weaponIds);

        Add("iron_mace", () => new HeavyWeapon("Iron Mace", 25, 5, new EquipTwoHanded()), _weaponIds);

        Add("great_excalibur", () =>
        {
            var sword = new HeavyWeapon("Great Excalibur", 40, 15, new EquipTwoHanded());
            sword.GrantedStats.AddModifier(StatType.Strength, new PercentModifier(0.5f));
            return sword;
        }, _weaponIds);

        Add("heavy_warhammer", () =>
        {
            var hammer = new HeavyWeapon("Heavy Warhammer", 75, 25, new EquipTwoHanded());
            hammer.GrantedStats.AddModifier(StatType.Strength, new FlatModifier(5));
            return hammer;
        }, _weaponIds);
        
        Add("gold_ingot", () => new Gold(50), _currencyIds);
        Add("small_coin_pouch", () => new Coin(15), _currencyIds);
        Add("single_coin", () => new Coin(1), _currencyIds);
        
        Add("old_bone", () => new MiscItem("Old Bone", 'b'), _miscIds);
        Add("mysterious_key", () => new MiscItem("Mysterious Key", 'k'), _miscIds);
        Add("torn_notebook", () => new MiscItem("Torn Calculus Notebook", 'n'), _miscIds);
        Add("random_pen", () => new MiscItem("Random Pen", 'p'), _miscIds);
    }

    public static IItem Create(string itemId)
    {
        if (_blueprints.TryGetValue(itemId, out var factoryMethod))
        {
            return factoryMethod.Invoke();
        }
        
        throw new ArgumentException($"Item with ID '{itemId}' does not exist in the factory!");
    }
    
    private static IItem ApplyRandomDecorators(IItem item, Random rng)
    {
        if (rng.NextDouble() < 0.1)
        {
            return new GodlyDecorator(item);
        }
        
        var count = rng.Next(0, 3);
        for (var i = 0; i < count; i++)
        {
            var choice = rng.Next(7);
            item = choice switch
            {
                0 => new StrongDecorator(item),
                1 => new UnluckyDecorator(item),
                2 => new AgileDecorator(item),
                _ => item
            };
        }
        return item;
    }
    
    public static IItem GetRandomItem(Random rng)
    {
        var keys = new List<string>(_blueprints.Keys);
        var randomIndex = rng.Next(keys.Count);
        return Create(keys[randomIndex]);
    }
    
    public static IItem GetRandomWeapon(Random rng)
    {
        if (_weaponIds.Count == 0)
            throw new InvalidOperationException("No weapons registered!");

        var baseItem = Create(_weaponIds[rng.Next(_weaponIds.Count)]);
        return ApplyRandomDecorators(baseItem, rng);
    }
    
    public static IItem GetRandomMisc(Random rng)
    {
        if (_miscIds.Count == 0)
            throw new InvalidOperationException("No misc items registered!");

        return Create(_miscIds[rng.Next(_miscIds.Count)]);
    }
    
}