using System;

namespace ConsoleRpg.Model.Systems.Logging;

public class LogEntry(string text, LogType type)
{
    public DateTime Timestamp { get; } = DateTime.Now;
    public string Text { get; } = text;
    public LogType Type { get; } = type;
    
    public override string ToString() => $"[{Timestamp}][{Type.ToString().ToUpper()}] {Text}\n";
}