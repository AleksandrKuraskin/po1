using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;

namespace ConsoleRpg.Core.Map.Themes;

public class VoidTheme : IThemeFactory
{
    public string ThemeId => "Void";
    public string IntroMessage => "There is nothing here. Only silence and the distant glimmer of dying stars.";
    
    private readonly LootTable<IItem> _items =
    [
        () => new MiscItem("Stardust", '*'),
        () => new MiscItem("Void Essence", 'v'),
        () => new MiscItem("Dark Matter Fragment", 'd'),
    ];

    private readonly LootTable<IItem> _weapons =
    [
        () => new LightWeapon("Orion's Belt", 10, 6, new EquipOneHanded()),
        () => new HeavyWeapon("Sirius Vanguard", 30, 12, new EquipTwoHanded()),
        () => new MagicWeapon("Nebula Core", 15, 15, new EquipOneHanded()),
    ];

    private readonly LootTable<Enemy> _enemies =
    [
        () => new Enemy("Sagittarius A*", 'O', 100, 20, 5),
        () => new Enemy("Void Aberration", 'V', 30, 12, 0),
        () => new Enemy("Phantom Star", 'S', 20, 15, 0),
    ];
    
    public void ApplyGenerationStrategy(IMapBuilder builder)
    {
        builder
            .StartEmptyDungeon()
            .AddSpecificItem(CreateArtifact())
            .AddItems(10, CreateRandomItem)
            .AddWeapons(2, CreateRandomWeapon)
            .AddEnemies(6, CreateEnemy);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("The Event Horizon", 100, 30, new EquipTwoHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        IItem baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.8);
    }

    public Enemy CreateEnemy(Random rng) => _enemies.GetRandom(rng);
}