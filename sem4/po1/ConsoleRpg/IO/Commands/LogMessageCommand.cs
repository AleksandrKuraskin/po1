using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;

namespace ConsoleRpg.IO.Commands;

public class LogMessageCommand(string message, LogType type) : ICommand
{
    public void Execute(Game game)
    {
        game.Logger.Log(message, type);
    }
}