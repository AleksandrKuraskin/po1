using ConsoleRpg.Systems.Logging.Loggers;

namespace ConsoleRpg.Systems.Logging;

public interface ILogNotifier
{
    void Attach(ILogListener listener);
    void Detach(ILogListener listener);
    void Notify(LogEntry entry);
}