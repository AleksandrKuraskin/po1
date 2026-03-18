using ConsoleRpg.Items.Currency;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Items;

public class ItemFactory
{
    private static readonly Dictionary<string, Func<IItem>> _blueprints = new();

    public static void Initialize()
    {
        _blueprints["rusty_sword"] = () =>
        {
            var sword = new OneHandedWeapon("Rusty Sword", 10, 2);
            return sword;
        };

        _blueprints["iron_mace"] = () =>
        {
            var mace = new OneHandedWeapon("Iron Mace", 25, 5);
            return mace;
        };
        
        _blueprints["great_excalibur"] = () =>
        {
            var sword = new TwoHandedWeapon("Great Excalibur", 40, 15);
            sword.Stats.Damage.AddModifier(new PercentModifier(0.5f)); 
            return sword;
        };

        _blueprints["heavy_warhammer"] = () =>
        {
            var hammer = new TwoHandedWeapon("Heavy Warhammer", 75, 25);
            hammer.Stats.Damage.AddModifier(new FlatModifier(5));
            return hammer;
        };
        
        _blueprints["gold_ingot"] = () => new Gold(50);
        _blueprints["small_coin_pouch"] = () => new Coin(15); 
        _blueprints["single_coin"] = () => new Coin(1);
        
        _blueprints["old_bone"] = () => new MiscItem("Old Bone", 'b');
        _blueprints["mysterious_key"] = () => new MiscItem("Mysterious Key", 'k');
        _blueprints["torn_notebook"] = () => new MiscItem("Torn Calculus Notebook", 'n');
        _blueprints["empty_bottle"] = () => new MiscItem("Empty Bottle", 'u');
    }

    public static IItem Create(string itemId)
    {
        if (_blueprints.TryGetValue(itemId, out var factoryMethod))
        {
            return factoryMethod.Invoke();
        }
        
        throw new ArgumentException($"Item with ID '{itemId}' does not exist in the factory!");
    }
    
    public static IItem GetRandomItem(Random rng)
    {
        var keys = new System.Collections.Generic.List<string>(_blueprints.Keys);
        var randomIndex = rng.Next(keys.Count);
        Console.WriteLine($"Random item: {keys[randomIndex]}");
        return Create(keys[randomIndex]);
    }
}