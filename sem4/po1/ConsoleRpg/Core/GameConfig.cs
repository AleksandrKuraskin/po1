using System.Text.Json.Serialization;

namespace ConsoleRpg.Core;

public class GameConfig(string path = Constants.DefaultConfigPath)
{
    [JsonIgnore] private readonly string _configPath = path;

    public string PlayerName { get; set; } = Constants.DefaultPlayerName;
    public string LogDirectory { get; set; } = Constants.DefaultLogsDirectory;
    
    public void Read(){}
}