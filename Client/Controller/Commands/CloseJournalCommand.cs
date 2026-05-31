using ConsoleRpg.Client.View;
using ConsoleRpg.Client.Controller.States;

namespace ConsoleRpg.Client.Controller.Commands;

public class CloseJournalCommand(IInputState previousState, IRenderer previousRenderer) : ILocalCommand
{
    public void ExecuteLocal(IClientModel model)
    {
        model.ChangeInputState(previousState);
        model.ChangeRenderer(previousRenderer);
    }
}
