using ConsoleRpg.Client.View.Components;
using Spectre.Console;

namespace ConsoleRpg.Client.View;

public class LogHistoryRenderer : IRenderer
{
    public void Render(IClientModel model)
    {
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Logs").Ratio(4),
                new Layout("Controls").Ratio(1)
            );

        var logsPanel = new LogsComponent(1).Build(model);
        var controlsRenderable = new ControlsComponent().Build(model);
        
        layout["Logs"].Update(logsPanel);
        layout["Controls"].Update(controlsRenderable);

        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }

    public void AddSidebarComponent(IUIComponent component) {}

    public void ClearSidebarComponents() {}
}
