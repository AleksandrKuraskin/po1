using ConsoleRpg.Core;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.IO.States;

namespace ConsoleRpg.IO.Commands;

public class ChangeStateCommand(GameHandler handler, IInputState newState, bool moveState) : ICommand
{
    public void Execute(Game game)
    {
        handler.SetState(newState);
        if (!moveState)
        {
            game.Logger.Log($"Inventory management state.");
        }
        else
        {
            game.Logger.Log($"Movement state.");
        }
    }
}