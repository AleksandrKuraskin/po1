using System.Text;
using ConsoleRpg.Shared.Systems.Logging;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class LogsComponent(int state = 0) : IUIComponent
{
    
    public string Name => "Logs";
    public IRenderable Build(IClientModel model)
    {
        var logBuilder = new StringBuilder();
        IEnumerable<LogEntry> visibleLogs;

        if (state == 1)
        {
            var maxLogs = Math.Max(1, Console.WindowHeight - 2);
            var allLogs = model.Logger.GetLogs();
            visibleLogs = allLogs.Skip(model.Logger.ScrollOffset).Take(maxLogs);
        }
        else
        {
            var maxLogs = Math.Max(1, (Console.WindowHeight / 2) - 2);
            visibleLogs = model.Logger.GetRecentLogs(maxLogs);
        }

        foreach (var log in visibleLogs)
        {
            var color = log.Type switch
            {
                LogType.Info => "white",
                LogType.Success => "green1",
                LogType.Warning => "orange1",
                LogType.Error => "red",
                LogType.Loot => "gold1",
                LogType.Combat => "maroon",
                LogType.Movement => "grey",
                LogType.Sound => "lightsteelblue1",
                LogType.Action => "aquamarine1",
                LogType.System => "slateblue1",
                _ => "white"
            };

            var timestamp = $"[grey][[{log.Timestamp:HH:mm:ss}]][/]";
            var entityPart = string.IsNullOrEmpty(log.Entity) ? "" : $" [bold cyan][[{Markup.Escape(log.Entity)}]][/]";
            logBuilder.AppendLine($"{timestamp}{entityPart} [{color}]{Markup.Escape(log.Text)}[/]");
        }
        return new Panel(new Markup(logBuilder.ToString())).Header("[bold]Logs[/]").RoundedBorder().Expand();
    }
}