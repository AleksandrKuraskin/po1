using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public class ActionHandler : InputHandlerBase
{
    public override void Handle(ConsoleKey key, Game game)
    {
        if (key == ConsoleKey.E)
        {
            var tile = game.Map.GetTile(game.Player.X, game.Player.Y);
            var item = tile.RemoveTopItem();
            if (item != null) item.OnPickUp(game.Player, game.Logger);
            else game.Logger.Log("No items to pick up.");
        }
        else if (key == ConsoleKey.Q)
        {
            game.Player.Inventory.DropModeActive = !game.Player.Inventory.DropModeActive;
            game.Logger.Log(game.Player.Inventory.DropModeActive ?
                "DROP MODE: Choose item from inventory to drop (0-9)." :
                "Drop mode disabled.");
        }
        else
        {
            base.Handle(key, game);
        }
    }
}