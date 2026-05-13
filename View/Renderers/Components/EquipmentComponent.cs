using ConsoleRpg.Model.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Renderers.Components;

public class EquipmentComponent : IUIComponent
{
    public string Name => "Equipment";
    public IRenderable Build(Game game)
    {
        var leftHandItem = game.Player.Equipment.LeftHand;
        var leftHandText = leftHandItem?.Name ?? "[grey]Empty[/]";
        var rightHandItem = game.Player.Equipment.RightHand;
        var rightHandText = rightHandItem?.Name ?? "[grey]Empty[/]";

        if (leftHandItem != null && leftHandItem == rightHandItem)
        {
            leftHandText = $"[magenta]{leftHandItem.Name}[/]";
            rightHandText = leftHandText;
        }

        var innerText = $"[bold]Left Hand (L):[/] {leftHandText}\n[bold]Right Hand (R):[/] {rightHandText}";
        return new Panel(new Markup(innerText)).Header("[bold blue]Equipment[/]").RoundedBorder().Expand();
    }
}