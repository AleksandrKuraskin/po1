using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class GameStateDto
{
    public PlayerDto LocalPlayer { get; set; } = new();
    public List<PlayerDto> OtherPlayers { get; set; } = [];
    public List<TileDto> ActiveTiles { get; set; } = [];
    public List<LogEntry> Logs { get; set; } = [];
    public bool Itemized { get; set; }
    public bool Dangerous { get; set; }
    public bool IsGameOver { get; set; }
}
