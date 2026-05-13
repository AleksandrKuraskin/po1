using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Logging;
using ConsoleRpg.Model.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Controller.Commands;

public class DropSelectedCommand: ICommand
{
    public void Execute(Game game)
    {
        var inv = game.Player.Inventory;
        var item = inv.RemoveItemAt(inv.SelectedIndex);

        if (item != null)
        {
            item.OnDrop(game.MapContext.Map, game.Player.X, game.Player.Y, item);
            if (item.Loudness > 0)
            {
                var sound = new DropSound(game.Player, item);
                game.Player.MakeNoise(sound);
            }
        }
        else LogManager.Instance.Log("This slot is empty. Nothing to drop.");
    }
}