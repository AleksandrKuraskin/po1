using System.Collections.Generic;

namespace ConsoleRpg.Model.Systems.Logging.Loggers;

public interface ILogger
{
    void Log(LogEntry entry);
    IEnumerable<LogEntry> GetRecentLogs(int count);
    IEnumerable<LogEntry> GetLogs();
}