using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Map.Procedures;
using ConsoleRpg.IO.Renderers.Components;

namespace ConsoleRpg.Core.Map;

public class MapBuilder : IBuilder
{
    private MapContext? _context;
    private readonly List<IMapProcedure> _procedures = new();

    public IBuilder StartFilledDungeon()
    {
        _context = new MapContext();
        _procedures.Clear();
        _procedures.Add(new FillMapProcedure());
        _context.SidebarComponents.Add(new StatsComponent()); 
        return this;
    }

    public IBuilder StartEmptyDungeon()
    {
        _context = new MapContext();
        _procedures.Clear();
        _procedures.Add(new EmptyMapProcedure());
        _context.SidebarComponents.Add(new StatsComponent());
        return this;
    }

    public IBuilder AddCentralHall(int w, int h) { _procedures.Add(new CentralHallProcedure(w, h)); return this; }
    public IBuilder AddCorridors() { _procedures.Add(new CorridorsProcedure()); return this; }
    public IBuilder AddRooms() { _procedures.Add(new RoomsProcedure()); return this; }
    public IBuilder AddWeapons(int count) { _procedures.Add(new WeaponsProcedure(count)); return this; }
    public IBuilder AddItems(int count) {_procedures.Add(new ItemProcedure(count)); return this;}
    public IBuilder AddEnemies(int count) {_procedures.Add(new EnemyProcedure(count)); return this;}

    public MapContext Build()
    {
        if (_context == null) throw new InvalidOperationException("Starting method must be called before building");
        foreach (var proc in _procedures) proc.Apply(_context);
        return _context;
    }
}