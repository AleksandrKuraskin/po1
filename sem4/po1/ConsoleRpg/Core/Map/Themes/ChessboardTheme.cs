using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Decorators;
using ConsoleRpg.Items.Weapons;

namespace ConsoleRpg.Core.Map.Themes;

public class ChessboardTheme : IThemeFactory
{
    public string ThemeId => "Chessboard";
    public string IntroMessage => "Black and white squares stretch into the horizon. Make your move.";

    private readonly LootTable<IItem> _items =
    [
        () => new MiscItem("Fallen pawn fragment", 'f'),
        () => new MiscItem("Shattered crown", 'c'),
        () => new MiscItem("Marble shard", 'm'),
    ];

    private readonly LootTable<IItem> _weapons =
    [
        () => new HeavyWeapon("Queen's Longsword", 20, 10, new EquipTwoHanded()),
        () => new LightWeapon("Sharp Knight's Hoof", 15, 5, new EquipOneHanded()),
        () => new MagicWeapon("Bishop's Scepter", 10, 8, new EquipOneHanded()),
    ];

    private readonly LootTable<Enemy> _enemies =
    [
        () => new Enemy("Black King", 'K', 100, 2, 5),
        () => new Enemy("Black Queen", 'Q', 200, 50, 20),
        () => new Enemy("Black Bishop", 'B', 60, 20, 8),
        () => new Enemy("Black Knight", 'N', 40, 15, 5),
        () => new Enemy("Black Rook", 'R', 60, 25, 10),
        () => new Enemy("Black Pawn", 'P', 20, 5, 2),
    ];

    public void ApplyGenerationStrategy(IMapDirector director)
    {
        director.ConstructRoom(this);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("The King's Checkmate", 50, 20, new EquipOneHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        IItem baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.6);
    }

    public Enemy CreateEnemy(Random rng) => _enemies.GetRandom(rng);
}