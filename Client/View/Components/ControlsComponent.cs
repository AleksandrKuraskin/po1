using ConsoleRpg.Shared.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class ControlsComponent : IUIComponent
{
    public string Name => "Controls";
    public IRenderable Build(IClientModel model)
    {
        var controlsString = model.CurrentInputState.GetInstructions();
        var innerText = $"[gray]{controlsString}[/]";
        return new Panel(new Markup(innerText)).Header("[bold grey]Controls[/]").RoundedBorder().Expand();
    }
}