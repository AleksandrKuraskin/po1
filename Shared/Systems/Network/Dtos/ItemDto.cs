using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class ItemDto
{
    public string Name { get; set; } = string.Empty;
    public char Symbol { get; set; }
    public int Quantity { get; set; }
    public List<string> Decorators { get; set; } = [];
    public Dictionary<StatType, StatDto> ItemStats { get; set; } = [];
    public Dictionary<StatType, StatDto> GrantedStats { get; set; } = [];
}
