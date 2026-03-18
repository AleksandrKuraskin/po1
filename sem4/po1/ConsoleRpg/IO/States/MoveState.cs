using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class MoveState: IInputState
{
    public ICommand HandleInput(ConsoleKey key, GameHandler handler)
    {
        return key switch
        {
            ConsoleKey.W => new MoveCommand(0, -1),
            ConsoleKey.S => new MoveCommand(0, 1),
            ConsoleKey.A => new MoveCommand(-1, 0),
            ConsoleKey.D => new MoveCommand(1, 0),
            ConsoleKey.E => new PickUpCommand(),
            ConsoleKey.I => new ChangeStateCommand(handler, new InventoryState(), false),
            _ => new NullCommand()
        };
    }
}