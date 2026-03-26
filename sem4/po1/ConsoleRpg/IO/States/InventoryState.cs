using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class InventoryState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputHandler _globalInputChain;
    
    public string Name { get; } = "Inventory Management State";

    public List<string> Instructions { get; } = new ()
    {
        "WS - Move Up/Down",
        "1-9 - Select Item",
        "Q - Drop Selected Item",
        "F - Swap Hands",
        "LP - Equip Left/Right Hand",
        "I - Close Inventory"
    };

    public InventoryState(IInputHandler globalChain)
    {
        _globalInputChain = globalChain;
        
        var prevItem = new KeyBindHandler(ConsoleKey.W, new NavigateInventoryCommand(-1));
        var itemAtIndex = new NumericBindHandler();
        var nextItem = new KeyBindHandler(ConsoleKey.S, new NavigateInventoryCommand(-2));
        var dropItem = new KeyBindHandler(ConsoleKey.Q, new DropSelectedCommand());
        var swapHands = new KeyBindHandler(ConsoleKey.F, new SwapHandsCommand());
        var equipLeft = new KeyBindHandler(ConsoleKey.L, new EquipSelectedCommand(true));
        var equipRight = new KeyBindHandler(ConsoleKey.P, new EquipSelectedCommand(false));
        var changeState = new KeyBindHandler(ConsoleKey.I, new ChangeStateCommand(this));

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
            output = output + " " + instruction;
        }
        return output;
    }
    
    public IInputState GetNewState(Game game) => new MoveState(game.MapContext, _globalInputChain);
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}