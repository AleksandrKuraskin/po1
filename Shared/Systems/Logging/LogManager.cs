using System.Collections.Generic;
using System.Linq;

namespace ConsoleRpg.Shared.Systems.Logging;

public class LogManager
{
    private static LogManager? _instance;
    private readonly List<ILogListener> _listeners = new();
    private readonly List<LogEntry> _logs = new();
    private long _nextId = 1;

    private LogManager(){}

    public static LogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LogManager();
            }
            return _instance;
        }
    }

    public long CurrentId => _nextId - 1;

    public void Attach(ILogListener listener)
    {
        _listeners.Add(listener);
    }
    
    public void Detach(ILogListener listener)
    {
        _listeners.Remove(listener);
    }

    public void Notify(LogEntry entry)
    {
        foreach (var listener in _listeners)
            listener.OnNotify(entry);
    }

    public void Log(string text, string? entity = null, string? recipientName = null, LogType type = LogType.Info)
    {
        var entry = new LogEntry(text, entity, recipientName, type, _nextId++);
        _logs.Add(entry);
        Notify(entry);
    }

    public IEnumerable<LogEntry> GetRecentLogs(int count) => _logs.TakeLast(count);
    public IEnumerable<LogEntry> GetLogsSince(long id) => _logs.Where(l => l.Id > id);
    
    public IEnumerable<LogEntry> GetLogsForPlayer(long lastId, string playerName)
    {
        return _logs.Where(l => l.Id > lastId && 
                                (l.RecipientName == null || l.RecipientName == playerName));
    }
}
