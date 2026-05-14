using ConsoleRpg.Model.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Components;

public class ControlsComponent : IUIComponent
{
    public string Name => "Controls";
    public IRenderable Build(Game game)
    {
        var controlsString = game.CurrentInputState.GetInstructions();
        var innerText = $"[gray]{controlsString}[/]";
        return new Panel(new Markup(innerText)).Header("[bold grey]Controls[/]").RoundedBorder().Expand();
    }
}