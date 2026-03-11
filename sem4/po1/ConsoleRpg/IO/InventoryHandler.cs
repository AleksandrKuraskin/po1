using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public class InventoryHandler : InputHandlerBase
{
    public override void Handle(ConsoleKey key, Game game)
    {
        if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
        {
            var slot = key == ConsoleKey.D0 ? 9 : (key - ConsoleKey.D1);
            
            if (game.DropModeActive)
            {
                var item = game.Player.Inventory.RemoveItem(slot);
                if (item != null) item.OnDrop(game.Map, game.Player.X, game.Player.Y, game.Logger);
                else game.Logger.Log("This slot is empty.");
                game.DropModeActive = false;
            }
            else
            {
                var item = game.Player.Inventory.GetItem(slot);
                if (item != null) item.TryEquip(game.Player, game.Logger);
                else game.Logger.Log("This slot is empty.");
            }
        }
        else
        {
            base.Handle(key, game);
        }
    }
}