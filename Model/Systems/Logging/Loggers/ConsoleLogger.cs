using System.Collections.Generic;
using System.Linq;

namespace ConsoleRpg.Model.Systems.Logging.Loggers;

public class ConsoleLogger : ILogger, ILogListener
{
    private readonly List<LogEntry> _logs = new ();
    
    public readonly int MaxLogCount = 20;
    public int ScrollOffset { get; set; } = 0;
    
    public void Log(LogEntry entry)
    {
        _logs.Add(entry);
    }
    public IEnumerable<LogEntry> GetRecentLogs(int count) => _logs.AsReadOnly().TakeLast(count);
    public IEnumerable<LogEntry> GetLogs() => _logs.AsReadOnly();
    
    public void OnNotify(LogEntry entry)
    {
        Log(entry);
    }
}