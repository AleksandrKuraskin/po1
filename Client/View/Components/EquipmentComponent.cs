using System.Linq;
using ConsoleRpg.Shared.Systems;
using Spectre.Console;
using Spectre.Console.Rendering;
using ConsoleRpg.Shared.Systems.Network.Dtos;

namespace ConsoleRpg.Client.View.Components;

public class EquipmentComponent : IUIComponent
{
    public string Name => "Equipment";
    public IRenderable Build(IClientModel model)
    {
        string leftHandText;
        string rightHandText;

        if (model.LastState != null)
        {
            var equip = model.LastState.LocalPlayer.Equipment;
            
            ItemDto? left = equip.TryGetValue(EquipmentSlot.LeftHand, out var l) ? l : null;
            ItemDto? right = equip.TryGetValue(EquipmentSlot.RightHand, out var r) ? r : null;

            leftHandText = FormatItem(left);
            rightHandText = FormatItem(right);

            if (left != null && right != null && left.Name == right.Name && left.Decorators.SequenceEqual(right.Decorators))
            {
                leftHandText = $"[magenta]{leftHandText}[/]";
                rightHandText = leftHandText;
            }
        }
        else
        {
            var leftHandItem = model.Player.Equipment.LeftHand;
            leftHandText = leftHandItem != null ? UIStyleRegistry.FormatItem(leftHandItem.GetState()) : "[grey]Empty[/]";
            var rightHandItem = model.Player.Equipment.RightHand;
            rightHandText = rightHandItem != null ? UIStyleRegistry.FormatItem(rightHandItem.GetState()) : "[grey]Empty[/]";

            if (leftHandItem != null && leftHandItem == rightHandItem)
            {
                leftHandText = $"[magenta]{leftHandText}[/]";
                rightHandText = leftHandText;
            }
        }

        var innerText = $"[bold]Left Hand (L):[/] {leftHandText}\n[bold]Right Hand (R):[/] {rightHandText}";
        return new Panel(new Markup(innerText)).Header("[bold blue]Equipment[/]").RoundedBorder().Expand();
    }

    private string FormatItem(ItemDto? item)
    {
        if (item == null) return "[grey]Empty[/]";
        return UIStyleRegistry.FormatItem(item);
    }
}