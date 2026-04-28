using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.Systems.Sound;

namespace ConsoleRpg.Core.Map;

public class MapContext
{
    public Map Map { get; set; } = new Map();
    public (int x, int y) SpawnPoint { get; set; } = (0, 0);

    public List<Room> Rooms = new();
    public bool Itemized { get; set; }
    public bool Dangerous { get; set; }
    public List<IUIComponent> SidebarComponents = new();
    public ISoundMediator? SoundMediator { get; set; }
}