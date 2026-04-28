using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.Items;
using ConsoleRpg.Systems.Sound;

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
    IMapBuilder AddEnemies(int count, Func<Random, ISoundMediator, Enemy> enemyMethod);
    IMapBuilder AddEnemyPack(int packSize, Func<Random, ISoundMediator, IEnumerable<Enemy>> packMethod);
    MapContext Build();
}