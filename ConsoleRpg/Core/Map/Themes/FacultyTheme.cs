using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Currency;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;

namespace ConsoleRpg.Core.Map.Themes;

public class FacultyTheme : IThemeFactory
{
    public string ThemeId => "Faculty";
    public string IntroMessage => "Your mind is consumed by the fear of the upcoming midterm (it’s tomorrow and you haven’t even studied).";
    
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
        () => new Enemy("Calculus midterm", 'C', 30, 10, 0),
        () => new Enemy("sgit.mini.pw.edu.pl", 'S', 15, 5, 2),
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

    public Enemy CreateEnemy(Random rng) => _enemies.GetRandom(rng);

}