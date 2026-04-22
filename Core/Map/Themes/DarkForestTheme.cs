using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;

namespace ConsoleRpg.Core.Map.Themes;

public class DarkForestTheme : IThemeFactory
{
    public string ThemeId => "DarkForest";
    public string IntroMessage => "Twisted branches block out the moonlight. You feel watched.";
    
    private readonly LootTable<IItem> _items =
    [
        () => new MiscItem("Poisonous Mushroom", 'm'),
        () => new MiscItem("Glowing Moss", 'g'),
        () => new MiscItem("Torn Ranger Cloak", 'c'),
    ];

    private readonly LootTable<IItem> _weapons =
    [
        () => new LightWeapon("Hunter's Bow", 15, 8, new EquipTwoHanded()),
        () => new MagicWeapon("Druid's Staff", 20, 10, new EquipTwoHanded()),
        () => new HeavyWeapon("Thorny Club", 25, 12, new EquipOneHanded()),
    ];

    private readonly LootTable<Enemy> _enemies =
    [
        () => new Enemy("Dire Wolf", 'W', 25, 12, 1),
        () => new Enemy("Corrupted Treant", 'T', 60, 15, 8),
        () => new Enemy("Shadow Weaver", 'S', 20, 10, 0),
    ];
    
    public void ApplyGenerationStrategy(IMapBuilder builder)
    {
        builder
            .StartFilledDungeon()
            .AddRooms()
            .AddCorridors()
            .AddSpecificItem(CreateArtifact())
            .AddItems(8, CreateRandomItem)
            .AddWeapons(4, CreateRandomWeapon)
            .AddEnemies(6, CreateEnemy);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("Heart of the Forest", 40, 15, new EquipOneHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        IItem baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.4);
    }

    public Enemy CreateEnemy(Random rng) => _enemies.GetRandom(rng);
}