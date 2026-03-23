using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class InventoryState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly string name = "Inventory Management State";

    public InventoryState()
    {
        var prevItem = new KeyBindHandler(ConsoleKey.W, new NavigateInventoryCommand(-1));
        var nextItem = new KeyBindHandler(ConsoleKey.S, new NavigateInventoryCommand(1));
        var dropItem = new KeyBindHandler(ConsoleKey.Q, new DropSelectedCommand());
        var swapHands = new KeyBindHandler(ConsoleKey.F, new SwapHandsCommand());
        var equipLeft = new KeyBindHandler(ConsoleKey.L, new EquipSelectedCommand(true));
        var equipRight = new KeyBindHandler(ConsoleKey.P, new EquipSelectedCommand(false));
        var changeState = new KeyBindHandler(ConsoleKey.I, new ChangeStateCommand(new MoveState(), true));

        prevItem
            .SetNext(nextItem)
            .SetNext(dropItem)
            .SetNext(swapHands)
            .SetNext(equipLeft)
            .SetNext(equipRight)
            .SetNext(changeState);
        
        _inputChain = prevItem;
    }
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}