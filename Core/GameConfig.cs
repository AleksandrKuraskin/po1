using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleRpg.Core;

public class GameConfig(string path = Constants.DefaultConfigPath)
{
    [JsonIgnore] private readonly string _configPath = path;
    [JsonIgnore] private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public string PlayerName { get; set; } = Constants.DefaultPlayerName;
    public string LogDirectory { get; set; } = Constants.DefaultLogsDirectory;

    public void Read()
    {
        if (!File.Exists(_configPath))
        {
            Write();
            return;
        }

        try
        {
            var json = File.ReadAllText(_configPath);
            var config = JsonSerializer.Deserialize<GameConfig>(json, _options);
            if (config != null)
            {
                PlayerName = config.PlayerName;
                LogDirectory = config.LogDirectory;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during reading config from file: {ex.Message}");
        }
    }
    
    public void Write()
    {
        try
        {
            var json = JsonSerializer.Serialize(this, _options);
            File.WriteAllText(_configPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during saving config to file: {ex.Message}");
        }
    }
}