using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class ItemDto
{
    public string? Name { get; set; }
    public char? Symbol { get; set; }
    public int? Quantity { get; set; }
    public Dictionary<StatType, StatDto> ItemStats { get; set; } = [];
    public Dictionary<StatType, StatDto> GrantedStats { get; set; } = [];
    
    public string? DecoratorId { get; set; }
    public ItemDto? Wrappee { get; set; }
}
