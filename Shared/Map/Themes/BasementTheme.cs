using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Entities.Enemies.Behaviors;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Currency;
using ConsoleRpg.Shared.Items.Decorators;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Map.Themes;

public class BasementTheme : IThemeFactory
{
    public string ThemeId => "Basement";
    public string IntroMessage => "The smell of damp earth and mold fills the air. Watch your step.";
    
    private static readonly SpeciesGroup _rats = new SpeciesGroup(2, new CowardlyBehavior());
    private static readonly SpeciesGroup _cats = new SpeciesGroup(4, new AgressiveBehavior());
    private static readonly SpeciesGroup _bats = new SpeciesGroup(1, new CowardlyBehavior());

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
        () => new Enemy("Rabid Rat", 'R', 10, 5, 0, _rats),
        () => new Enemy("Feral Cat", 'C', 15, 8, 1, _cats),
        () => new Enemy("Cave Bat", 'B', 8, 4, 0, _bats),
    ];

    public void ApplyGenerationStrategy(IMapDirector director)
    {
        director.ConstructRandom(this);
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