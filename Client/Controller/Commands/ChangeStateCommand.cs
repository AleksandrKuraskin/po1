using ConsoleRpg.Client.Controller.States;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Client.Controller.Commands;

public class ChangeStateCommand(IInputState currentState) : ILocalCommand
{
    private readonly IInputState _currentState = currentState;

    public void ExecuteLocal(IClientModel model)
    {
        var newState = _currentState.GetNewState(model);
        model.ChangeInputState(newState);
        LogManager.Instance.Log($"{newState.Name}", type: LogType.System);
    }
}
