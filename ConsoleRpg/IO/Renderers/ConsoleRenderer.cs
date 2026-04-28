using Spectre.Console;
using ConsoleRpg.Core;
using ConsoleRpg.IO.Renderers.Components;


namespace ConsoleRpg.IO.Renderers;

public class ConsoleRenderer : IRenderer
{
    public void Render(Game game)
    {
        var currentTile = game.MapContext.Map.GetTile(game.Player.X, game.Player.Y);
        var context = game.MapContext;
        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Left").Ratio(2),
                new Layout("Center").Ratio(3),
                new Layout("Right").Ratio(2)
            );
        
        layout["Left"].SplitRows(
            new Layout("Stats").Ratio(4),
            new Layout("Equipment").Size(4).Invisible(),
            new Layout("Inventory").Ratio(6).Invisible()
        );

        layout["Center"].SplitRows(
            new Layout("Map").MinimumSize(20),
            new Layout("Logs")
        );

        layout["Right"].SplitRows(
            new Layout("Enemy").Invisible(),
            new Layout("Ground").Invisible(),
            new Layout("Controls")
        );
        
        // Static components
        layout["Map"].Update(new MapComponent().Build(game));
        layout["Logs"].Update(new LogsComponent().Build(game));
        layout["Controls"].Update(new ControlsComponent().Build(game));
        
        // Dynamic components
        foreach (var component in context.SidebarComponents)
        {
            var rendered = component.Build(game);
            if (rendered != null) layout[component.Name].Visible().Update(rendered);
        }
        
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}