using System;
using System.Collections.Generic;
using Spectre.Console;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.View.Components;


namespace ConsoleRpg.Client.View;

public class ConsoleRenderer : IRenderer
{
    private readonly List<IUIComponent> _sidebarComponents = new();

    public void AddSidebarComponent(IUIComponent component)
    {
        _sidebarComponents.Add(component);
    }

    public void ClearSidebarComponents()
    {
        _sidebarComponents.Clear();
    }

    public void Render(IClientModel model)
    {
        var currentTile = model.MapContext.Map.GetTile(model.Player.X, model.Player.Y);
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
            new Layout("Players").Invisible(),
            new Layout("Ground").Invisible(),
            new Layout("Controls")
        );
        
        // Static components
        layout["Map"].Update(new MapComponent().Build(model));
        layout["Logs"].Update(new LogsComponent().Build(model));
        layout["Controls"].Update(new ControlsComponent().Build(model));
        
        // Dynamic sidebar components
        foreach (var component in _sidebarComponents)
        {
            var rendered = component.Build(model);
            if (rendered != null) layout[component.Name].Visible().Update(rendered);
        }
        
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}
