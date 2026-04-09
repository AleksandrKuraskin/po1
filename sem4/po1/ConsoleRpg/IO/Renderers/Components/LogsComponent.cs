using System.Text;
using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class LogsComponent : IUIComponent
{
    
    public string Name => "Logs";
    public IRenderable Build(Game game)
    {
        var logBuilder = new StringBuilder();
        var logs = game.Logger.GetLogQueue();

        foreach (var log in logs)
        {
            var color = log.Type switch
            {
                LogType.Info => "white",
                LogType.Success => "green",
                LogType.Warning => "orange1",
                LogType.Error => "red",
                LogType.Loot => "gold1",
                _ => "white"
            };
            logBuilder.AppendLine($"[{color}]> {Markup.Escape(log.Text)}[/]");
        }
        
        for (var i = logs.Count; i < game.Logger.MaxSize; i++) logBuilder.AppendLine();
        
        return new Panel(new Markup(logBuilder.ToString())).Header("[bold]Logs[/]").RoundedBorder().Expand();
    }
}