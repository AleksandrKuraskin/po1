using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class InventoryState: IInputState
{
    public ICommand HandleInput(ConsoleKey key, GameHandler handler)
    {
        return key switch
        {
            ConsoleKey.W => new NavigateInventoryCommand(-1),
            ConsoleKey.S => new NavigateInventoryCommand(1),
            ConsoleKey.Q => new DropSelectedCommand(),
            ConsoleKey.F => new SwapHandsCommand(),
            ConsoleKey.L => new EquipSelectedCommand(isLeftHand: true),
            ConsoleKey.P => new EquipSelectedCommand(isLeftHand: false),
            ConsoleKey.I => new ChangeStateCommand(handler, new MoveState(), true),
            _ => new NullCommand()
        };
    }
}