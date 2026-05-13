using ConsoleRpg.Model.Core;
using ConsoleRpg.Controller.Handlers;
using ConsoleRpg.Controller.States;
using ConsoleRpg.Model.Systems.Logging;

namespace ConsoleRpg.Controller.Commands;

public class ChangeStateCommand(IInputState currentState) : ICommand
{
    public void Execute(Game game)
    {
        var newState = currentState.GetNewState(game);
        game.ChangeInputState(newState);;
        LogManager.Instance.Log($"{newState.Name}");
    }
}