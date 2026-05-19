using System;
using System.Linq;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.View;
using ConsoleRpg.Client.Controller.States;

namespace ConsoleRpg.Client.Controller.Commands;

public class OpenJournalCommand(IInputState previousState) : ILocalCommand
{
    public void ExecuteLocal(IClientModel model)
    {
        var previousRenderer = model.Renderer;
        
        var logState = new LogHistoryState(previousState, previousRenderer, model.GlobalInputHandler, model.GlobalInstructions);
        
        var totalLogs = model.Logger.GetLogs().Count();
        var maxLogs = Math.Max(model.Logger.MaxLogCount, Console.WindowHeight - 4);
        
        model.Logger.ScrollOffset = Math.Max(0, totalLogs - maxLogs);
        
        model.ChangeInputState(logState);
        model.ChangeRenderer(new LogHistoryRenderer());
    }
}
