using ConsoleRpg.Core;
using ConsoleRpg.Systems.Logging;

namespace ConsoleRpg.IO.Commands;

public class LogMessageCommand(string message, LogType type) : ICommand
{
    public void Execute(Game game)
    {
        LogManager.Instance.Log(message, type);
    }
}