using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class ExitGameCommand : ICommand
{
    public void Execute(Game game)
    {
        game.Exit();
    }
}