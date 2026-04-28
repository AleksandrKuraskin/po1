using ConsoleRpg.Core.Map.Themes;

namespace ConsoleRpg.Core.Map;

public class MapDirector(IMapBuilder builder) : IMapDirector
{
    private readonly IMapBuilder _builder = builder;

    public void ConstructRandom(IThemeFactory theme)
    {
        _builder
            .StartFilledDungeon()
            .AddRooms()
            .AddCorridors()
            .AddSpecificItem(theme.CreateArtifact())
            .AddItems(8, theme.CreateRandomItem)
            .AddWeapons(4, theme.CreateRandomWeapon)
            .AddEnemies(5, theme.CreateEnemy);
    }

    public void ConstructRoom(IThemeFactory theme)
    {
        _builder
            .StartFilledDungeon()
            .AddCentralHall(30, 16)
            .AddSpecificItem(theme.CreateArtifact())
            .AddItems(5, theme.CreateRandomItem)
            .AddWeapons(5, theme.CreateRandomWeapon)
            .AddEnemies(10, theme.CreateEnemy);
    }

    public void ConstructEmpty(IThemeFactory theme)
    {
        _builder
            .StartEmptyDungeon()
            .AddSpecificItem(theme.CreateArtifact())
            .AddItems(10, theme.CreateRandomItem)
            .AddWeapons(2, theme.CreateRandomWeapon)
            .AddEnemies(6, theme.CreateEnemy);
    }
}