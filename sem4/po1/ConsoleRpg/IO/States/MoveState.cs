using ConsoleRpg.Core;
using ConsoleRpg.Core.Map;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class MoveState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputHandler _globalInputChain;
    private readonly List<ActionInfo> _globalInstructions;
    
    public string Name { get; } = "Move State";

    public List<ActionInfo> Instructions { get; }

    public MoveState(MapContext context, IInputHandler globalChain, List<ActionInfo> globalInstructions)
    {
        _globalInputChain = globalChain;
        _globalInstructions = globalInstructions;

        Instructions = _globalInstructions;
        
        var moveUp = new KeyBindHandler(new ActionInfo(ConsoleKey.W, new MoveCommand(0, -1), "Move up"), Instructions);
        var moveDown = new KeyBindHandler(new ActionInfo(ConsoleKey.S, new MoveCommand(0, 1), "Move down"), Instructions);
        var moveLeft = new KeyBindHandler(new ActionInfo(ConsoleKey.A, new MoveCommand(-1, 0), "Move left"), Instructions);
        var moveRight = new KeyBindHandler(new ActionInfo(ConsoleKey.D, new MoveCommand(1, 0), "Move right"), Instructions);
        
        moveUp
            .SetNext(moveDown)
            .SetNext(moveLeft)
            .SetNext(moveRight);
        if (context.Itemized)
        {
            var swapHands = new KeyBindHandler(new ActionInfo(ConsoleKey.F, new SwapHandsCommand(), "Swap hands"), Instructions);
            var pickUp = new KeyBindHandler(new ActionInfo(ConsoleKey.E, new PickUpCommand(), "Pick up"), Instructions);
            var changeState = new KeyBindHandler(new ActionInfo(ConsoleKey.I, new ChangeStateCommand(this), "Inventory management"), Instructions);
            moveRight
                .SetNext(swapHands)
                .SetNext(pickUp)
                .SetNext(changeState)
                .SetNext(_globalInputChain);
        }
        else
        {
            moveRight.SetNext(_globalInputChain);
        }
        
        _inputChain = moveUp;
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

    public IInputState GetNewState(Game game) => new InventoryState(_globalInputChain, _globalInstructions);
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}