using ConsoleRpg.Model.Systems.Logging.Loggers;

namespace ConsoleRpg.Model.Systems.Logging;

public interface ILogNotifier
{
    void Attach(ILogListener listener);
    void Detach(ILogListener listener);
    void Notify(LogEntry entry);
}