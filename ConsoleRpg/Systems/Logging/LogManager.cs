using System.Collections.Generic;

namespace ConsoleRpg.Systems.Logging;

public class LogManager : ILogNotifier
{
    private static LogManager? _instance;
    private readonly List<ILogListener> _listeners = new();

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

    public void Log(string message, LogType type = LogType.Info)
    {
        var entry = new LogEntry(message, type);
        Notify(entry);
    }
}