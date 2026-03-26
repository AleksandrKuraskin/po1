using ConsoleRpg.Core;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.IO.States;

namespace ConsoleRpg.IO.Commands;

public class ChangeStateCommand(IInputState currentState) : ICommand
{
    public void Execute(Game game)
    {
        var newState = currentState.GetNewState(game);
        game.ChangeInputState(newState);;
        game.Logger.Log($"{newState.Name}");
    }
}