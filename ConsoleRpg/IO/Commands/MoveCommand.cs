using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class MoveCommand(int dx, int dy) : ICommand
{
    private readonly int _dx = dx;
    private readonly int _dy = dy;

    public void Execute(Game game)
    {
        game.MapContext.Map.TryMove(game.Player, _dx, _dy);
    }
}