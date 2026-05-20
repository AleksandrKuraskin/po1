using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Entities.Enemies.Behaviors;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Currency;
using ConsoleRpg.Shared.Items.Decorators;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Map.Themes;

public class FacultyTheme : IThemeFactory
{
    public string ThemeId => "Faculty";
    public string IntroMessage => "Your mind is consumed by the fear of the upcoming midterm (it’s tomorrow and you haven’t even studied).";

    private static readonly SpeciesGroup _repos = new SpeciesGroup(20, new AgressiveBehavior());
    private static readonly SpeciesGroup _midterms = new SpeciesGroup(10, new AgressiveBehavior());
    
    private readonly LootTable<IItem> _items = 
    [
        () => new MiscItem("Torn calculus notebook", 'n'),
        () => new MiscItem("Broken laptop", 'l'),
        () => new Gold(10),
    ];

    private readonly LootTable<IItem> _weapons = 
    [
        () => new MagicWeapon("Discrete mathematics book", 5, 2, new EquipTwoHanded()),
        () => new LightWeapon("Tiny pen", 3, 4, new EquipOneHanded()),
    ];

    private readonly LootTable<Enemy> _enemies =
    [
        () => new Enemy("Calculus midterm", 'C', 30, 10, 0, _midterms),
        () => new Enemy("sgit Repository", 'S', 15, 5, 2, _repos),
    ];
    
    public void ApplyGenerationStrategy(IMapDirector director)
    {
        director.ConstructRandom(this);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("AiSD midterm answers sheet", 25, 5, new EquipOneHanded());
        return new GodlyDecorator(artifact); 
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        var baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, 0.5);
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