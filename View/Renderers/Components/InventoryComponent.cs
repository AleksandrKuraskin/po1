using System.Text;
using ConsoleRpg.Model.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Renderers.Components;

public class InventoryComponent : IUIComponent
{
    public string Name => "Inventory";
    public IRenderable Build(Game game)
    {
        var invBuilder = new StringBuilder();
        var inv = game.Player.Inventory;
        var items = inv.GetItems();
        var invCap = inv.Capacity;

        for (var i = 0; i < invCap; i++)
        {
            var itemName = items[i]?.Name ?? "---";
            var slotText = $"[[{(i + 1) % invCap}]] {Markup.Escape(itemName)}";

            if (i == game.Player.Inventory.SelectedIndex)
            {
                invBuilder.AppendLine($"[green]> {slotText} <[/]");
            }
            else
            {
                if (items[i] == null)
                    invBuilder.AppendLine($"[grey]{slotText}[/]");
                else
                    invBuilder.AppendLine($"{slotText}");
            }
        }
        return new Panel(new Markup(invBuilder.ToString())).Header("[bold green]Inventory[/]").RoundedBorder().Expand();
    }
}