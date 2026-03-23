using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class MoveState: IInputState
{
    private readonly IInputHandler _inputChain;

    public MoveState()
    {
        var moveUp = new KeyBindHandler(ConsoleKey.W, new MoveCommand(0, -1));
        var moveDown = new KeyBindHandler(ConsoleKey.S, new MoveCommand(0, 1));
        var moveLeft = new KeyBindHandler(ConsoleKey.A, new MoveCommand(-1, 0));
        var moveRight = new KeyBindHandler(ConsoleKey.D, new MoveCommand(1, 0));
        var swapHands = new KeyBindHandler(ConsoleKey.F, new SwapHandsCommand());
        var pickUp = new KeyBindHandler(ConsoleKey.E, new PickUpCommand());
        var changeState = new KeyBindHandler(ConsoleKey.I, new ChangeStateCommand(new InventoryState(), false));
        moveUp
            .SetNext(moveDown)
            .SetNext(moveLeft)
            .SetNext(moveRight)
            .SetNext(swapHands)
            .SetNext(pickUp)
            .SetNext(changeState);

        _inputChain = moveUp;
    }

    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}