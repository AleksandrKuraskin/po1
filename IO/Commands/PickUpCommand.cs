using ConsoleRpg.Core;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Sound.SoundEvents;

namespace ConsoleRpg.IO.Commands;

public class PickUpCommand : ICommand
{
    public void Execute(Game game)
    {
        var tile = game.MapContext.Map.GetTile(game.Player.X, game.Player.Y);
        var item = tile.GetTopItem();
        if (item != null)
        {
            var picked = item.TryPickUp(game.Player, item);
            if (picked)
            {
                tile.RemoveTopItem();
                if (item.Loudness > 0)
                {
                    var sound = new PickUpSound(game.Player, item);
                    game.Player.MakeNoise(sound);
                }
            }
        }
        else LogManager.Instance.Log("No items to pick up.", LogType.Warning);
    }
}