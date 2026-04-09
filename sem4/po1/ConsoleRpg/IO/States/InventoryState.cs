using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class InventoryState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputHandler _globalInputChain;
    private readonly List<ActionInfo> _globalInstructions;
    
    public string Name { get; } = "Inventory Management State";

    public List<ActionInfo> Instructions { get; }

    public InventoryState(IInputHandler globalChain, List<ActionInfo> globalInstructions)
    {
        _globalInputChain = globalChain;
        _globalInstructions = globalInstructions;
        Instructions = new List<ActionInfo>(_globalInstructions);
        
        
        var prevItem = new KeyBindHandler(new ActionInfo(ConsoleKey.W, new NavigateInventoryCommand(-1), "Prev item"), Instructions);
        var itemAtIndex = new NumericBindHandler();
        var nextItem = new KeyBindHandler(new ActionInfo(ConsoleKey.S, new NavigateInventoryCommand(-2), "Next item"), Instructions);
        var dropItem = new KeyBindHandler(new ActionInfo(ConsoleKey.Q, new DropSelectedCommand(), "Drop item"), Instructions);
        var swapHands = new KeyBindHandler(new ActionInfo(ConsoleKey.F, new SwapHandsCommand(), "Swap hands"), Instructions);
        var equipLeft = new KeyBindHandler(new ActionInfo(ConsoleKey.L, new EquipSelectedCommand(true), "Equip left"), Instructions);
        var equipRight = new KeyBindHandler(new ActionInfo(ConsoleKey.P, new EquipSelectedCommand(false), "Equip right"), Instructions);
        var changeState = new KeyBindHandler(new ActionInfo(ConsoleKey.I, new ChangeStateCommand(this), "Movement state"), Instructions);

        prevItem
            .SetNext(nextItem)
            .SetNext(itemAtIndex)
            .SetNext(dropItem)
            .SetNext(swapHands)
            .SetNext(equipLeft)
            .SetNext(equipRight)
            .SetNext(changeState)
            .SetNext(_globalInputChain);
        
        _inputChain = prevItem;
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
    
    public IInputState GetNewState(Game game) => new MoveState(game.MapContext, _globalInputChain, _globalInstructions);
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}