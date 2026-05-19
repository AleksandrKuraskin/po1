using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Sound;

namespace ConsoleRpg.Shared.Maps;

public class MapContext
{
    public Map Map { get; set; } = new Map();
    public (int x, int y) SpawnPoint { get; set; } = (0, 0);

    public List<Room> Rooms = new();
    public bool Itemized { get; set; }
    public bool Dangerous { get; set; }
    public ISoundMediator? SoundMediator { get; set; }
}
