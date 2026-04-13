namespace ConsoleRpg.Core.Map;

public class MapDirector(IMapBuilder mapBuilder)
{
    private readonly IMapBuilder _mapBuilder = mapBuilder;

    public void ConstructRandomMap()
    {
        _mapBuilder
            .StartFilledDungeon()
            .AddCentralHall(8, 4)
            .AddRooms()
            .AddCorridors()
            .AddWeapons(20)
            .AddItems(10)
            .AddEnemies(5);
    }

    public void ConstructEmptyMap()
    {
        _mapBuilder
            .StartEmptyDungeon()
            .AddCentralHall(38, 18);
    }
}