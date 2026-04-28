using System.Collections.Generic;

namespace ConsoleRpg.Core.Logger;

public class Logger
{
    private readonly Queue<LogMessage> _logQueue = new();
    public readonly int MaxSize = 20;

    public void Log(string message, LogType type = LogType.Info)
    {
        
        if (_logQueue.Count >= MaxSize)
            _logQueue.Dequeue();
        _logQueue.Enqueue(new LogMessage(message, type));
    }
    public Queue<LogMessage> GetLogQueue() => _logQueue;
}