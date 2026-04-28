using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Entities.Enemies.Behaviors;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;
using ConsoleRpg.Systems.Sound;

namespace ConsoleRpg.Core.Map.Themes;

public class DarkForestTheme : IThemeFactory
{
    public string ThemeId => "DarkForest";
    public string IntroMessage => "Twisted branches block out the moonlight. You feel watched.";

    private static readonly SpeciesGroup _beasts = new SpeciesGroup(3, new AgressiveBehavior());
    private static readonly SpeciesGroup _plants = new SpeciesGroup(5, new CowardlyBehavior());
    
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
        () => new Enemy("Dire Wolf", 'W', 25, 12, 1, _beasts),
        () => new Enemy("Corrupted Treant", 'T', 60, 15, 8, _plants),
        () => new Enemy("Shadow Weaver", 'S', 20, 10, 0, _plants),
    ];
    
    public void ApplyGenerationStrategy(IMapDirector director)
    {
        director.ConstructRandom(this);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("Heart of the Forest", 40, 15, new EquipOneHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        var baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.4);
    }

    public Enemy CreateEnemy(Random rng, ISoundMediator mediator)
    {
        var enemy = _enemies.GetRandom(rng);
        enemy.SetMediator(mediator);
        return enemy;
    }
    
    public IEnumerable<Enemy> CreateEnemyPack(Random rng, ISoundMediator mediator)
    {
        var recipe = _enemies.GetRandomMethod(rng);
        var packSize = rng.Next(2, 5);
        var pack = new List<Enemy>();

        for (var i = 0; i < packSize; i++)
        {
            var enemy = recipe.Invoke();
            enemy.SetMediator(mediator);
            pack.Add(enemy);
        }
        return pack;
    }
}