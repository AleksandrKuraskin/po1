using System;
using System.Collections.Generic;
using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Entities.Enemies.Behaviors;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Items.Decorators;
using ConsoleRpg.Shared.Items.Weapons;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Maps.Themes;

public class VoidTheme : IThemeFactory
{
    public string ThemeId => "Void";
    public string IntroMessage => "There is nothing here. Only silence and the distant glimmer of dying stars.";

    private static readonly SpeciesGroup _darkstars = new SpeciesGroup(20, new AgressiveBehavior());
    
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
        () => new Enemy("Sagittarius A*", 'O', 100, 20, 5, _darkstars),
        () => new Enemy("Void Aberration", 'V', 30, 12, 0, _darkstars),
        () => new Enemy("Phantom Star", 'S', 20, 15, 0, _darkstars),
    ];
    
    public void ApplyGenerationStrategy(IMapDirector director)
    {
        director.ConstructEmpty(this);
    }

    public IItem CreateArtifact()
    {
        var artifact = new MagicWeapon("The Event Horizon", 100, 30, new EquipTwoHanded());
        return new GodlyDecorator(artifact);
    }

    public IItem CreateRandomItem(Random rng) => _items.GetRandom(rng);

    public IItem CreateRandomWeapon(Random rng)
    {
        var baseWeapon = _weapons.GetRandom(rng);
        return DecoratorRegistry.ApplyRandomDecorators(baseWeapon, rng, chanceToEnchant: 0.8);
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