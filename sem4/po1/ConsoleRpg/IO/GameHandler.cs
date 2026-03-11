using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public class GameHandler : InputHandlerBase
{
    public override void Handle(ConsoleKey key, Game game)
    {
        if (key == ConsoleKey.Escape)
        {
            game.Exit();
        }
        else
        {
            base.Handle(key, game);
        }
    }
}