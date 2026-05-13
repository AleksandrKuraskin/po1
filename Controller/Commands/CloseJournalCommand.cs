using ConsoleRpg.Model.Core;
using ConsoleRpg.View.Renderers;
using ConsoleRpg.Controller.States;

namespace ConsoleRpg.Controller.Commands;

public class CloseJournalCommand(IInputState previousState, IRenderer previousRenderer) : ICommand
{
    public void Execute(Game game)
    {
        game.ChangeInputState(previousState);
        game.ChangeRenderer(previousRenderer);
    }
}