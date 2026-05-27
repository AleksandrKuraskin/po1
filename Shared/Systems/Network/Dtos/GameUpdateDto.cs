using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Systems.Network.Dtos;

public class GameUpdateDto
{
    public PlayerDto LocalPlayer { get; set; } = new();
    public List<PlayerDto> OtherPlayers { get; set; } = [];
    public List<TileDto> UpdatedTiles { get; set; } = [];
    public List<LogEntry> Logs { get; set; } = [];
    public bool IsGameOver { get; set; }
}