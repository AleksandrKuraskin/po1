namespace ConsoleRpg.Core.Map;

public class MapDirector(IMapBuilder builder)
{
    private readonly IMapBuilder _builder = builder;

    public MapContext ConstructRandomMap()
    {
        return _builder
            .StartFilledDungeon()
            .AddCentralHall(8, 4)
            .AddRooms()
            .AddCorridors()
            .AddWeapons(3)
            .AddItems(10)
            .Build();
    }

    public MapContext ConstructEmptyMap()
    {
        return _builder
            .StartEmptyDungeon()
            .AddCentralHall(38, 18)
            .Build();
    }
}