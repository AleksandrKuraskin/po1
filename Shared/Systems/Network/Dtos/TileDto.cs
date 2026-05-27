using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class TileDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public bool IsWall { get; set; }
    public string? EnemyName { get; set; }
    public Dictionary<StatType, StatDto> EnemyStats { get; set; } = [];
    public List<ItemDto> Items { get; set; } = [];
    public List<string> PlayerNames { get; set; } = [];
}
