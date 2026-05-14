using System;
using System.Linq;
using ConsoleRpg.Model.Core;
using ConsoleRpg.View;
using ConsoleRpg.Controller.States;

namespace ConsoleRpg.Controller.Commands;

public class OpenJournalCommand(IInputState previousState) : ICommand
{
    public void Execute(Game game)
    {
        var previousRenderer = game.Renderer;
        
        var logState = new LogHistoryState(previousState, previousRenderer, game.GlobalInputHandler, game.GlobalInstructions);
        
        var totalLogs = game.Logger.GetLogs().Count();
        var maxLogs = Math.Max(game.Logger.MaxLogCount, Console.WindowHeight - 4);
        
        game.Logger.ScrollOffset = Math.Max(0, totalLogs - maxLogs);
        
        game.ChangeInputState(logState);
        game.ChangeRenderer(new LogHistoryRenderer());
    }
}