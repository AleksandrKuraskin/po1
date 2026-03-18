using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class NavigateInventoryCommand(int direction) : ICommand
{
    public void Execute(Game game)
    {
        var inv = game.Player.Inventory;
        inv.SelectedIndex = Math.Clamp(inv.SelectedIndex + direction, 0, inv.Capacity - 1);
    }
}