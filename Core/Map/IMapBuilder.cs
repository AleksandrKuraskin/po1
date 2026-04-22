using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;

namespace ConsoleRpg.Core.Map;

public interface IMapBuilder
{
    IMapBuilder StartFilledDungeon();
    IMapBuilder StartEmptyDungeon();
    IMapBuilder AddCentralHall(int width, int height);
    IMapBuilder AddCorridors();
    IMapBuilder AddRooms();
    IMapBuilder AddWeapons(int count, Func<Random, IItem> weaponMethod);
    IMapBuilder AddItems(int count, Func<Random, IItem> itemMethod);
    IMapBuilder AddSpecificItem(IItem item);
    IMapBuilder AddEnemies(int count, Func<Random, Enemy> enemyMethod);
    MapContext Build();
}