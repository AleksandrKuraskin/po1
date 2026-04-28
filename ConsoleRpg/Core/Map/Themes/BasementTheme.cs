using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Currency;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;

namespace ConsoleRpg.Core.Map.Themes;

public class BasementTheme : IThemeFactory
{
    public string ThemeId => "Basement";
    public string IntroMessage => "The smell of damp earth and mold fills the air. Watch your step.";
    
    private readonly LootTable<IItem> _items =
    [
        () => new MiscItem("Dirty sock", 's'),
        () => new MiscItem("Rotten plank", 'p'),
        () => new MiscItem("Rusty spring", 'r'),
        () => new Coin(5),
    ];

    private readonly LootTable<IItem> _weapons =
    [
        () => new HeavyWeapon("Wooden chair", 10, 4, new EquipTwoHanded()),
        () => new HeavyWeapon("Rusty chainsaw", 15, 8, new EquipTwoHanded()),
        () => new LightWeapon("Spirit level", 5, 3, new EquipOneHanded()),
    ];

    private readonly LootTable<Enemy> _enemies =
    [
        () => new Enemy("Rabid Rat", 'R', 10, 5, 0),
        () => new Enemy("Feral Cat", 'C', 15, 8, 1),
        () => new Enemy("Cave Bat", 'B', 8, 4, 0),
    ];
    
    public void ApplyGenerationStrategy(IMapBuilder builder)
    {
        builder
            .StartFilledDungeon()
            .AddRooms()
            .AddCorridors()
            .AddSpecificItem(CreateArtifact())
            .AddItems(10, CreateRandomItem)
            .AddWeapons(3, CreateRandomWeapon)
            .AddEnemies(8, CreateEnemy);
    }

    public IItem CreateArtifact()
    {
        var artifact = new HeavyWeapon("Grandpa's Antique Shotgun", 30, 15, new EquipTwoHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        var baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.3);
    }

    public Enemy CreateEnemy(Random rng) => _enemies.GetRandom(rng);
}