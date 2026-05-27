using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class InventoryComponent : IUIComponent
{
    public string Name => "Inventory";
    public IRenderable Build(IClientModel model)
    {
        var invBuilder = new StringBuilder();
        
        if (model.LastState != null)
        {
            var items = model.LastState.LocalPlayer.Inventory;
            var invCap = 10;
            var selectedIndex = model.Player.Inventory.SelectedIndex;

            for (var i = 0; i < invCap; i++)
            {
                var itemDto = items.Count > i ? items[i] : null;
                var formattedName = itemDto != null ? UIStyleRegistry.FormatItem(itemDto) : "---";
                var slotText = $"[[{(i + 1) % invCap}]] {formattedName}";

                if (i == selectedIndex)
                    invBuilder.AppendLine($"[green]> {slotText} <[/]");
                else
                    invBuilder.AppendLine(itemDto == null ? $"[grey]{slotText}[/]" : $"{slotText}");
            }
        }
        else
        {
            var inv = model.Player.Inventory;
            var items = inv.GetItems();
            var invCap = inv.Capacity;
            var selectedIndex = inv.SelectedIndex;

            for (var i = 0; i < invCap; i++)
            {
                var item = items[i];
                var formattedName = item != null ? UIStyleRegistry.FormatItem(item.GetState()) : "---";
                var slotText = $"[[{(i + 1) % invCap}]] {formattedName}";

                if (i == selectedIndex)
                    invBuilder.AppendLine($"[green]> {slotText} <[/]");
                else
                    invBuilder.AppendLine(item == null ? $"[grey]{slotText}[/]" : $"{slotText}");
            }
        }
        return new Panel(new Markup(invBuilder.ToString())).Header("[bold green]Inventory[/]").RoundedBorder().Expand();
    }
}