using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Logging;

namespace ConsoleRpg.Controller.Commands;

public class LogMessageCommand(string message, LogType type) : ICommand
{
    public void Execute(Game game)
    {
        LogManager.Instance.Log(message, type);
    }
}