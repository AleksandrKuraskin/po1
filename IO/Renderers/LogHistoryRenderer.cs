using System;
using System.Text;
using ConsoleRpg.Core;
using ConsoleRpg.IO.Renderers.Components;
using Spectre.Console;

namespace ConsoleRpg.IO.Renderers;

public class LogHistoryRenderer : IRenderer
{
    public void Render(Game game)
    {
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Logs").Ratio(4),
                new Layout("Controls").Ratio(1)
            );

        var logsPanel = new LogsComponent(1).Build(game);
        var controlsRenderable = new ControlsComponent().Build(game);
        
        layout["Logs"].Update(logsPanel);
        layout["Controls"].Update(controlsRenderable);

        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}