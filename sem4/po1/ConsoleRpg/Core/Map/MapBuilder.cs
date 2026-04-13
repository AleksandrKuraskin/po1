using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Map.Procedures;
using ConsoleRpg.IO.Renderers.Components;

namespace ConsoleRpg.Core.Map;

public class MapBuilder : IMapBuilder
{
    private MapContext? _context;
    private readonly List<IMapProcedure> _procedures = new();

    public IMapBuilder StartFilledDungeon()
    {
        _context = new MapContext();
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
    public IMapBuilder AddWeapons(int count) { _procedures.Add(new WeaponsProcedure(count)); return this; }
    public IMapBuilder AddItems(int count) {_procedures.Add(new ItemProcedure(count)); return this;}
    public IMapBuilder AddEnemies(int count) {_procedures.Add(new EnemyProcedure(count)); return this;}

    public MapContext Build()
    {
        if (_context == null) throw new InvalidOperationException("Starting method must be called before building");
        foreach (var proc in _procedures) proc.Apply(_context);
        return _context;
    }
}