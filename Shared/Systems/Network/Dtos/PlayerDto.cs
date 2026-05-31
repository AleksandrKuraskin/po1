using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class PlayerDto
{
    public string Name { get; set; } = "";
    public int PlayerNumber { get; set; }
    public char Symbol { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Dictionary<StatType, StatDto> Stats { get; set; } = [];
    public int Gold { get; set; }
    public int Coins { get; set; }
    public List<ItemDto?> Inventory { get; set; } = [];
    public Dictionary<EquipmentSlot, ItemDto> Equipment { get; set; } = [];
}
