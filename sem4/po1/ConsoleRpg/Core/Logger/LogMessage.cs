namespace ConsoleRpg.Core.Logger;

public class LogMessage(string text, LogType type)
{
    public string Text { get; } = text;
    public LogType Type { get; } = type;
}