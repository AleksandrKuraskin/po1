using System;
using System.Text;
using Spectre.Console;
using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.Systems;


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
            new Layout("Equipment").Size(4),
            new Layout("Inventory").Ratio(6)
        );

        layout["Center"].SplitRows(
            new Layout("Map").MinimumSize(20),
            new Layout("Logs")
        );

        layout["Right"].SplitRows(
            new Layout("Enemy"),
            new Layout("Ground"),
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
            if (rendered != null) layout[component.Name].Update(rendered);
        }
        
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}