using System;
using System.Collections.Generic;
using ConsoleRpg.Model.Entities.Enemies;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Systems.Sound;

namespace ConsoleRpg.Model.Core.Map.Themes;

public interface IThemeFactory
{
    public string ThemeId { get; }
    public string IntroMessage { get; }
    
    void ApplyGenerationStrategy(IMapDirector director);
    
    IItem CreateRandomItem(Random rng);
    IItem CreateRandomWeapon(Random rng);
    IItem CreateArtifact();
    Enemy CreateEnemy(Random rng, ISoundMediator mediator);
    IEnumerable<Enemy> CreateEnemyPack(Random rng, ISoundMediator mediator);
    
}