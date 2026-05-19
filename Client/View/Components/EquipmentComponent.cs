using ConsoleRpg.Shared.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

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
            leftHandText = equip.TryGetValue("LeftHand", out var left) ? left : "[grey]Empty[/]";
            rightHandText = equip.TryGetValue("RightHand", out var right) ? right : "[grey]Empty[/]";

            if (leftHandText != "[grey]Empty[/]" && leftHandText == rightHandText)
            {
                leftHandText = $"[magenta]{Markup.Escape(leftHandText)}[/]";
                rightHandText = leftHandText;
            }
            else
            {
                if (leftHandText != "[grey]Empty[/]") leftHandText = Markup.Escape(leftHandText);
                if (rightHandText != "[grey]Empty[/]") rightHandText = Markup.Escape(rightHandText);
            }
        }
        else
        {
            var leftHandItem = model.Player.Equipment.LeftHand;
            leftHandText = leftHandItem?.Name ?? "[grey]Empty[/]";
            var rightHandItem = model.Player.Equipment.RightHand;
            rightHandText = rightHandItem?.Name ?? "[grey]Empty[/]";

            if (leftHandItem != null && leftHandItem == rightHandItem)
            {
                leftHandText = $"[magenta]{Markup.Escape(leftHandItem.Name)}[/]";
                rightHandText = leftHandText;
            }
            else
            {
                if (leftHandItem != null) leftHandText = Markup.Escape(leftHandItem.Name);
                if (rightHandItem != null) rightHandText = Markup.Escape(rightHandItem.Name);
            }
        }

        var innerText = $"[bold]Left Hand (L):[/] {leftHandText}\n[bold]Right Hand (R):[/] {rightHandText}";
        return new Panel(new Markup(innerText)).Header("[bold blue]Equipment[/]").RoundedBorder().Expand();
    }
}