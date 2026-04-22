using ConsoleRpg.Core;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.States;

namespace ConsoleRpg.IO.Commands;

public class CloseJournalCommand(IInputState previousState, IRenderer previousRenderer) : ICommand
{
    public void Execute(Game game)
    {
        game.ChangeInputState(previousState);
        game.ChangeRenderer(previousRenderer);
    }
}