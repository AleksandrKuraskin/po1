using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class DropSelectedCommand: ICommand
{
    public void Execute(Game game)
    {
        var inv = game.Player.Inventory;
        var item = inv.RemoveItemAt(inv.SelectedIndex);
        
        if (item != null) item.OnDrop(game.MapContext.Map, game.Player.X, game.Player.Y, game.Logger);
        else game.Logger.Log("This slot is empty. Nothing to drop.");
    }
}