using System;
using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Controller.Commands;

public class NavigateInventoryCommand(int index) : ICommand
{
    public void Execute(Game game)
    {
        var inv = game.Player.Inventory;
        if (index < 0)
        {
            var direction = index == -1 ? -1 : 1;
            inv.SelectedIndex = Math.Clamp(inv.SelectedIndex + direction, 0, inv.Capacity - 1);
        }
        else
        {
            inv.SelectedIndex = index;
        }
    }
}