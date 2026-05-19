using ConsoleRpg.Shared.Maps.Themes;

namespace ConsoleRpg.Shared.Maps;

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
            .AddEnemyPack(3, theme.CreateEnemyPack);
    }

    public void ConstructRoom(IThemeFactory theme)
    {
        _builder
            .StartFilledDungeon()
            .AddCentralHall(30, 16)
            .AddSpecificItem(theme.CreateArtifact())
            .AddItems(5, theme.CreateRandomItem)
            .AddWeapons(5, theme.CreateRandomWeapon)
            .AddEnemyPack(2, theme.CreateEnemyPack);
    }

    public void ConstructEmpty(IThemeFactory theme)
    {
        _builder
            .StartEmptyDungeon()
            .AddSpecificItem(theme.CreateArtifact())
            .AddItems(10, theme.CreateRandomItem)
            .AddWeapons(2, theme.CreateRandomWeapon)
            .AddEnemyPack(1, theme.CreateEnemyPack);
    }
}