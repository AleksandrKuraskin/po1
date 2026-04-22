using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;

namespace ConsoleRpg.Core.Map.Themes;

public interface IThemeFactory
{
    public string ThemeId { get; }
    public string IntroMessage { get; }
    
    void ApplyGenerationStrategy(IMapBuilder builder);
    
    IItem CreateRandomItem(Random rng);
    IItem CreateRandomWeapon(Random rng);
    IItem CreateArtifact();
    Enemy CreateEnemy(Random rng);
    
}