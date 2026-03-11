namespace ConsoleRpg.Core;

public class Logger
{
    private readonly Queue<string> _logQueue = new();
    private const int MaxSize = 5;

    public void Log(string message)
    {
        if (_logQueue.Count >= MaxSize)
            _logQueue.Dequeue();
        _logQueue.Enqueue(message);
    }

    public override string ToString()
    {
        var str = "";
        foreach (var log in _logQueue)
        {
            str = str + log + "\n";
        }
        return str;
    }
}