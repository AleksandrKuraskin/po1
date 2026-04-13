namespace ConsoleRpg.Core.Map;

public interface IMapBuilder
{
    IMapBuilder StartFilledDungeon();
    IMapBuilder StartEmptyDungeon();
    IMapBuilder AddCentralHall(int width, int height);
    IMapBuilder AddCorridors();
    IMapBuilder AddRooms();
    IMapBuilder AddWeapons(int count);
    IMapBuilder AddItems(int count);
    IMapBuilder AddEnemies(int count);
    MapContext Build();
}