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
        var mapPanel = new MapComponent().Build(game);
        var sidebarGrid = new Grid().AddColumn(new GridColumn());
        var context = game.MapContext;

        foreach (var component in context.SidebarComponents)
        {
            sidebarGrid.AddRow(component.Build(game));
        }
        
        var logPanel = new LogsComponent().Build(game);
        var controlsPanel = new ControlsComponent().Build(game);

        var mainGrid = new Grid()
            .AddColumn(new GridColumn())
            .AddRow(mapPanel)
            .AddRow(logPanel)
            .AddRow(controlsPanel);

        var layout = new Grid()
            .AddColumn(new GridColumn().Width(45))
            .AddColumn(new GridColumn())
            .AddRow(mainGrid, sidebarGrid);
        
        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}