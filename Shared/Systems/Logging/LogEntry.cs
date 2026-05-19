using System;

namespace ConsoleRpg.Shared.Systems.Logging;

public class LogEntry(string text, LogType type, LogScope scope = LogScope.Global, string? targetPlayer = null, string? origin = null, long id = 0)
{
    public long Id { get; set; } = id;
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Text { get; } = text;
    public LogType Type { get; } = type;
    public LogScope Scope { get; } = scope;
    public string? TargetPlayer { get; } = targetPlayer;
    public string? Origin { get; } = origin;
    
    public override string ToString() 
    {
        var originPart = string.IsNullOrEmpty(Origin) ? "" : $"[{Origin}]";
        return $"[{Timestamp:HH:mm:ss}]{originPart}[{Type.ToString().ToUpper()}] {Text}\n";
    }
}
