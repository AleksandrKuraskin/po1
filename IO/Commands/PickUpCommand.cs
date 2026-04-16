using ConsoleRpg.Core;
using ConsoleRpg.Systems.Logging;

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
            }
        }
        else LogManager.Instance.Log("No items to pick up.", LogType.Warning);
    }
}