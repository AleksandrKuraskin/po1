using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Shared.Systems.Network;

public class GameState
{
    public PlayerDto LocalPlayer { get; set; } = new();
    public List<PlayerDto> OtherPlayers { get; set; } = [];
    public List<TileDto> ActiveTiles { get; set; } = [];
    public List<LogEntry> Logs { get; set; } = [];
    public bool Itemized { get; set; }
    public bool Dangerous { get; set; }
    public bool IsGameOver { get; set; }
}

public class PlayerDto
{
    public string Name { get; set; } = "";
    public int X { get; set; }
    public int Y { get; set; }
    public Dictionary<string, StatDto> Stats { get; set; } = [];
    public int Gold { get; set; }
    public int Coins { get; set; }
    public List<string> Inventory { get; set; } = [];
    public Dictionary<string, string> Equipment { get; set; } = [];
}

public class StatDto
{
    public int BaseValue { get; set; }
    public int Value { get; set; }
}

public class TileDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public bool IsWall { get; set; }
    public string? EnemyName { get; set; }
    public Dictionary<string, int> EnemyStats { get; set; } = [];
    public List<string> ItemNames { get; set; } = [];
    public List<string> PlayerNames { get; set; } = [];
}
