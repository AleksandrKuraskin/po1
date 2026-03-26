using ConsoleRpg.Items.Currency;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Items;

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
        Add("rusty_sword", () => new OneHandedWeapon("Rusty Sword", 10, 2), _weaponIds);

        Add("iron_mace", () => new OneHandedWeapon("Iron Mace", 25, 5), _weaponIds);

        Add("great_excalibur", () =>
        {
            var sword = new TwoHandedWeapon("Great Excalibur", 40, 15);
            sword.Stats.Damage.AddModifier(new PercentModifier(0.5f));
            return sword;
        }, _weaponIds);

        Add("heavy_warhammer", () =>
        {
            var hammer = new TwoHandedWeapon("Heavy Warhammer", 75, 25);
            hammer.Stats.Damage.AddModifier(new FlatModifier(5));
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

        return Create(_weaponIds[rng.Next(_weaponIds.Count)]);
    }
    
    public static IItem GetRandomMisc(Random rng)
    {
        if (_miscIds.Count == 0)
            throw new InvalidOperationException("No misc items registered!");

        return Create(_miscIds[rng.Next(_miscIds.Count)]);
    }
    
}