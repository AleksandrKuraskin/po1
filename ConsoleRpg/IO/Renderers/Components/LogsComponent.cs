using System.Text;
using ConsoleRpg.Core;
using ConsoleRpg.Systems.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class LogsComponent(int state = 0) : IUIComponent
{
    
    public string Name => "Logs";
    public IRenderable Build(Game game)
    {
        var logBuilder = new StringBuilder();
        IEnumerable<LogEntry> visibleLogs;

        if (state == 1)
        {
            var maxLogs = Math.Max(1, Console.WindowHeight - 2);
            var allLogs = game.Logger.GetLogs();
            visibleLogs = allLogs.Skip(game.Logger.ScrollOffset).Take(maxLogs);
        }
        else
        {
            var maxLogs = Math.Max(1, (Console.WindowHeight / 2) - 2);
            visibleLogs = game.Logger.GetRecentLogs(maxLogs);
        }

        foreach (var log in visibleLogs)
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
            
            logBuilder.AppendLine($"[grey][[{log.Timestamp:HH:mm:ss}]][/] [{color}]{Markup.Escape(log.Text)}[/]");
        }
        return new Panel(new Markup(logBuilder.ToString())).Header("[bold]Logs[/]").RoundedBorder().Expand();
    }
}