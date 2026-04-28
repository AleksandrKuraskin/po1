using ConsoleRpg.Core;
using ConsoleRpg.Systems.Sound.SoundEvents;

namespace ConsoleRpg.IO.Commands;

public class MoveCommand(int dx, int dy) : ICommand
{
    private readonly int _dx = dx;
    private readonly int _dy = dy;

    public void Execute(Game game)
    {
        var player = game.Player;
        if (game.MapContext.Map.TryMovePlayer(player, _dx, _dy))
        {
            var sound = new MoveSound(player);
            player.MakeNoise(sound);
        }
    }
}