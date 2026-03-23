using ConsoleRpg.Core;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.IO.States;

namespace ConsoleRpg.IO.Commands;

public class ChangeStateCommand(IInputState newState, bool moveState) : ICommand
{
    public void Execute(Game game)
    {
        game.ChangeInputState(newState);
        var stateName = moveState ? "Move State" : "Inventory Management State";
        game.Logger.Log($"{stateName}");
    }
}