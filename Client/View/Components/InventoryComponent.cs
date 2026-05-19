using System.Text;
using ConsoleRpg.Shared.Core;
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
            var invCap = 10; // Default capacity or get from state if added
            var selectedIndex = model.Player.Inventory.SelectedIndex;

            for (var i = 0; i < invCap; i++)
            {
                var itemName = i < items.Count ? items[i] : "---";
                var slotText = $"[[{(i + 1) % invCap}]] {Markup.Escape(itemName)}";

                if (i == selectedIndex)
                {
                    invBuilder.AppendLine($"[green]> {slotText} <[/]");
                }
                else
                {
                    if (i >= items.Count)
                        invBuilder.AppendLine($"[grey]{slotText}[/]");
                    else
                        invBuilder.AppendLine($"{slotText}");
                }
            }
        }
        else
        {
            var inv = model.Player.Inventory;
            var items = inv.GetItems();
            var invCap = inv.Capacity;

            for (var i = 0; i < invCap; i++)
            {
                var itemName = items[i]?.Name ?? "---";
                var slotText = $"[[{(i + 1) % invCap}]] {Markup.Escape(itemName)}";

                if (i == model.Player.Inventory.SelectedIndex)
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
        }
        return new Panel(new Markup(invBuilder.ToString())).Header("[bold green]Inventory[/]").RoundedBorder().Expand();
    }
}