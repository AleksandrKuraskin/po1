namespace ConsoleRpg.Systems.Logging.Loggers;

public class FileLogger : ILogger, ILogListener
{
    private readonly List<LogEntry> _logs = new ();
    private readonly string _filePath;

    public FileLogger(string playerName, string logDirectory, string logFileName)
    {
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }
        
        _filePath = Path.Combine(logDirectory, logFileName);
        File.WriteAllText(_filePath, $"--- {playerName}'s journal | start: {DateTime.Now} ---\n");
        
    }
    
    public IEnumerable<LogEntry> GetRecentLogs(int count) => _logs.AsReadOnly().TakeLast(count);
    public IEnumerable<LogEntry> GetLogs() => _logs.AsReadOnly();

    public void Log(LogEntry entry)
    {
        _logs.Add(entry);
        var logString = entry + "\n";
        File.AppendAllText(_filePath, logString);
    }

    public void OnNotify(LogEntry entry)
    {
        Log(entry);
    }
}