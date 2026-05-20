using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Map.Themes;

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