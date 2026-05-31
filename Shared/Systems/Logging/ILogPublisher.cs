using ConsoleRpg.Shared.Systems.Logging.Loggers;

namespace ConsoleRpg.Shared.Systems.Logging;

public interface ILogNotifier
{
    void Attach(ILogListener listener);
    void Detach(ILogListener listener);
    void Notify(LogEntry entry);
}