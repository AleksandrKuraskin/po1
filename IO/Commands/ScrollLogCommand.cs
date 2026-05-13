using System;
using System.Linq;
using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class ScrollLogCommand(int direction) : ICommand
{
    public void Execute(Game game)
    {
        var totalLogs = game.Logger.GetLogs().Count();
        var maxLogs = Math.Max(1, Console.WindowHeight - 4);
        
        game.Logger.ScrollOffset = Math.Clamp(game.Logger.ScrollOffset + direction, 0, Math.Max(0, totalLogs - maxLogs));
    }
}