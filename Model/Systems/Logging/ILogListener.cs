namespace ConsoleRpg.Model.Systems.Logging;

public interface ILogListener
{
    public void OnNotify(LogEntry entry);
}