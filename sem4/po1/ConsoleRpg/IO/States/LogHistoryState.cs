using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.IO.Renderers;

namespace ConsoleRpg.IO.States;

public class LogHistoryState : IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputState _previousState;
    
    public string Name => "Log History";
    public List<ActionInfo> Instructions { get; }

    public LogHistoryState(IInputState previousState, IRenderer previousRenderer, IInputHandler globalChain, List<ActionInfo> globalInstructions)
    {
        _previousState = previousState;
        Instructions = new List<ActionInfo>(globalInstructions);
        
        var quitLog = new KeyBindHandler(
            new ActionInfo(ConsoleKey.Q, new CloseJournalCommand(previousState, previousRenderer), "Close Journal"), 
            Instructions
        );
        
        
        var scrollUp = new KeyBindHandler(
            new ActionInfo(ConsoleKey.UpArrow, new ScrollLogCommand(-1), "Scroll Up"), 
            Instructions
        );

        var scrollDown = new KeyBindHandler(
            new ActionInfo(ConsoleKey.DownArrow, new ScrollLogCommand(1), "Scroll Down"), 
            Instructions
        );

        scrollUp
            .SetNext(scrollDown)
            .SetNext(quitLog)
            .SetNext(globalChain);
        
        _inputChain = scrollUp;
    }

    public string GetInstructions()
    {
        var output = "";
        foreach (var instruction in Instructions)
        {
            var instructionString = instruction.Key + " - " + instruction.Description;
            output = output + "\n" + instructionString;
        }
        return output;
    }
    
    public IInputState GetNewState(Game game) => _previousState; 
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}