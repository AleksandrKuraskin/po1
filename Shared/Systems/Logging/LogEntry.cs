using System;

namespace ConsoleRpg.Shared.Systems.Logging;

public class LogEntry(string text, string? entity = null, string? recipientName = null, LogType type = LogType.Info, long id = 0)
{
    public long Id { get; set; } = id;
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Text { get; } = text;
    public string? Entity { get; } = entity;
    public string? RecipientName { get; } = recipientName;
    public LogType Type { get; } = type;
    
    public override string ToString() 
    {
        var entityPart = string.IsNullOrEmpty(Entity) ? "" : $" [{Entity}]";
        return $"[{Timestamp:HH:mm:ss}]{entityPart} {Text}";
    }
}
