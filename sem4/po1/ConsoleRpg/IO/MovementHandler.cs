using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public class MovementHandler  : InputHandlerBase
{
    public override void Handle(ConsoleKey key, Game game)
    {
        switch (key)
        {
            case ConsoleKey.W: case ConsoleKey.UpArrow: game.Map.TryMove(game.Player, 0, -1); break;
            case ConsoleKey.S: case ConsoleKey.DownArrow: game.Map.TryMove(game.Player, 0, 1); break;
            case ConsoleKey.A: case ConsoleKey.LeftArrow: game.Map.TryMove(game.Player, -1, 0); break;
            case ConsoleKey.D: case ConsoleKey.RightArrow: game.Map.TryMove(game.Player, 1, 0); break;
            default: base.Handle(key, game); break;
        }
    }
}