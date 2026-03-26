using ConsoleRpg.Core;
using ConsoleRpg.Core.Map;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class MoveState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputHandler _globalInputChain;
    
    public string Name { get; } = "Move State";

    public List<string> Instructions { get; }

    public MoveState(MapContext context, IInputHandler globalChain)
    {
        _globalInputChain = globalChain;
        
        var moveUp = new KeyBindHandler(ConsoleKey.W, new MoveCommand(0, -1));
        var moveDown = new KeyBindHandler(ConsoleKey.S, new MoveCommand(0, 1));
        var moveLeft = new KeyBindHandler(ConsoleKey.A, new MoveCommand(-1, 0));
        var moveRight = new KeyBindHandler(ConsoleKey.D, new MoveCommand(1, 0));
        
        if (context.Itemized)
        {
            var swapHands = new KeyBindHandler(ConsoleKey.F, new SwapHandsCommand());
            var pickUp = new KeyBindHandler(ConsoleKey.E, new PickUpCommand());
            var changeState = new KeyBindHandler(ConsoleKey.I, new ChangeStateCommand(this));
            moveUp
                .SetNext(moveDown)
                .SetNext(moveLeft)
                .SetNext(moveRight)
                .SetNext(swapHands)
                .SetNext(pickUp)
                .SetNext(changeState)
                .SetNext(_globalInputChain);
            
            Instructions = new List<string>
            {
                "WSAD-Move",
                "F-Swap Hands",
                "E-Pick Up",
                "I-Inventory Management"
            };
        }
        else
        {
            moveUp
                .SetNext(moveDown)
                .SetNext(moveLeft)
                .SetNext(moveRight)
                .SetNext(_globalInputChain);
            Instructions = new List<string>
            {
                "WSAD-Move"
            };
        }
        
        _inputChain = moveUp;
    }
    
    public string GetInstructions()
    {
        var output = "";
        foreach (var instruction in Instructions)
        {
            output = output + " " + instruction;
        }
        return output;
    }

    public IInputState GetNewState(Game game) => new InventoryState(_globalInputChain);
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}