using ConsoleRpg.Client.View;
using ConsoleRpg.Client.Controller.States;

namespace ConsoleRpg.Client.Controller.Commands;

public class OpenPlayersMenuCommand(IInputState previousState) : ILocalCommand
{
    public void ExecuteLocal(IClientModel model)
    {
        var previousRenderer = model.Renderer;
        
        model.ChangeInputState(new PlayersMenuState(previousState, previousRenderer, model.GlobalInputHandler, model.GlobalInstructions));
        model.ChangeRenderer(new PlayersMenuRenderer());
    }
}
