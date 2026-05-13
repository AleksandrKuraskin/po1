using System;
using System.Collections.Generic;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Map.Procedures;
using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.Items;
using ConsoleRpg.Systems.Sound;

namespace ConsoleRpg.Core.Map;

public class MapBuilder : IMapBuilder
{
    private MapContext? _context;
    private readonly List<IMapProcedure> _procedures = new();

    public IMapBuilder StartFilledDungeon()
    {
        _context = new MapContext();
        _context.SoundMediator = new SoundManager(_context.Map);
        _procedures.Clear();
        _procedures.Add(new FillMapProcedure());
        _context.SidebarComponents.Add(new StatsComponent()); 
        return this;
    }

    public IMapBuilder StartEmptyDungeon()
    {
        _context = new MapContext();
        _procedures.Clear();
        _procedures.Add(new EmptyMapProcedure());
        _context.SidebarComponents.Add(new StatsComponent());
        return this;
    }

    public IMapBuilder AddCentralHall(int w, int h) { _procedures.Add(new CentralHallProcedure(w, h)); return this; }
    public IMapBuilder AddCorridors() { _procedures.Add(new CorridorsProcedure()); return this; }
    public IMapBuilder AddRooms() { _procedures.Add(new RoomsProcedure()); return this; }

    public IMapBuilder AddWeapons(int count, Func<Random, IItem> weaponMethod)
    {
        _procedures.Add(new WeaponsProcedure(count, weaponMethod));
        return this;
    }

    public IMapBuilder AddItems(int count, Func<Random, IItem> itemMethod)
    {
        _procedures.Add(new ItemProcedure(count, itemMethod));
        return this;
    }

    public IMapBuilder AddSpecificItem(IItem item)
    {
        _procedures.Add(new SpecificItemProcedure(item));
        return this;
    }

    public IMapBuilder AddEnemies(int count, Func<Random, ISoundMediator, Enemy> enemyMethod)
    {
        if (_context == null)
            throw new NullReferenceException("Context must be started before adding enemies");
        
        _context.SoundMediator = new SoundManager(_context.Map);
        
        _procedures.Add(new EnemyProcedure(count, enemyMethod, _context.SoundMediator));
        return this;
    }

    public IMapBuilder AddEnemyPack(int packSize, Func<Random, ISoundMediator, IEnumerable<Enemy>> packMethod)
    {
        if (_context == null)
            throw new NullReferenceException("Context must be started before adding enemies");
        
        _context.SoundMediator = new SoundManager(_context.Map);
        
        _procedures.Add(new EnemyPackProcedure(packSize, packMethod, _context.SoundMediator));
        return this;
    }

    public MapContext Build()
    {
        if (_context == null) throw new InvalidOperationException("Starting method must be called before building");
        foreach (var proc in _procedures) proc.Apply(_context);
        return _context;
    }
}