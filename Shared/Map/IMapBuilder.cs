using System;
using System.Collections.Generic;
using ConsoleRpg.Shared.Entities.Enemies;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Maps;

public interface IMapBuilder
{
    IMapBuilder StartFilledDungeon();
    IMapBuilder StartEmptyDungeon();
    IMapBuilder AddCentralHall(int w, int h);
    IMapBuilder AddCorridors();
    IMapBuilder AddRooms();
    IMapBuilder AddWeapons(int count, Func<Random, IItem> weaponMethod);
    IMapBuilder AddItems(int count, Func<Random, IItem> itemMethod);
    IMapBuilder AddSpecificItem(IItem item);
    IMapBuilder AddEnemies(int count, Func<Random, ISoundMediator, Enemy> enemyMethod);
    IMapBuilder AddEnemyPack(int packSize, Func<Random, ISoundMediator, IEnumerable<Enemy>> packMethod);
    MapContext Build();
}
