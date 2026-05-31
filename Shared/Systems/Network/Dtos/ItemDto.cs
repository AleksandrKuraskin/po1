using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class ItemDto
{
    // Base item fields
    public string? Name { get; set; }
    public char? Symbol { get; set; }
    public int? Quantity { get; set; }
    public Dictionary<StatType, StatDto> ItemStats { get; set; } = [];
    public Dictionary<StatType, StatDto> GrantedStats { get; set; } = [];

    // Decorator fields
    public string? DecoratorId { get; set; }
    public ItemDto? Wrappee { get; set; }
}
