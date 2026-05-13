using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Controller.Commands;

public class ExitGameCommand : ICommand
{
    public void Execute(Game game)
    {
        game.Exit();
    }
}