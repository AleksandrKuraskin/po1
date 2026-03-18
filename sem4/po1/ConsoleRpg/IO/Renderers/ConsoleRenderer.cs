using System;
using System.Text;
using Spectre.Console;
using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.Entities;


namespace ConsoleRpg.IO.Renderers;

public class ConsoleRenderer : IRenderer
{
    public void Render(Game game)
    {
        Console.SetCursorPosition(0, 0);

        var layout = new Grid();
        layout.AddColumn(new GridColumn().PadRight(2));
        layout.AddColumn(new GridColumn());            
        
        var mapBuilder = new StringBuilder();
        for (var y = 0; y < game.Map.Height; y++)
        {
            for (var x = 0; x < game.Map.Width; x++)
            {
                if (game.Player.X == x && game.Player.Y == y)
                    mapBuilder.Append("[blue]¶[/]");
                else
                {
                    var symbol = game.Map.GetTile(x, y).GetSymbol();
                    switch (symbol)
                    {
                        case '█':
                            mapBuilder.Append("[grey]█[/]");
                            break;
                        case ' ':
                            mapBuilder.Append(" ");
                            break;
                        default:
                            mapBuilder.Append($"[gold1]{symbol}[/]");
                            break;
                    }
                }
            }
            mapBuilder.AppendLine();
        }
        var mapPanel = new Panel(new Markup(mapBuilder.ToString())).Header("[bold cyan]Dungeon Map[/]").Expand();

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
        
        var logPanel = new Panel(new Markup(logBuilder.ToString()))
            .Header("[bold]Logs[/]")
            .Expand();

        var leftColumn = new Rows(mapPanel, logPanel);
        var baseDamage = game.Player.Stats.Strength?.Value ?? 0;
        var eqDamage = game.Player.Equipment.GetTotalDamage();
        var totalDamage = baseDamage + eqDamage;

        var statsContent = $@"
        [bold]Health:[/]    [green]{game.Player.Stats.MaxHealth?.Value ?? 100,-4}[/]/[green] 100[/]
        [bold]Damage:[/]    [red]{totalDamage,-4}[/] [grey](Base: {baseDamage} + Eq: {eqDamage})[/]
        [bold]Intelligence:[/]  [blue]{game.Player.Stats.Intelligence?.Value ?? 10,-4}[/]
        [bold]Agility:[/] [gold1]{game.Player.Stats.Agility?.Value ?? 10,-4}[/]
        [bold]Luck:[/] [yellow]{game.Player.Stats.Luck?.Value ?? 10,-4}[/]

        [bold gold1]Gold:[/] {game.Player.Wallet.GoldValue,-5} | [bold silver]Coins:[/] {game.Player.Wallet.CoinValue,-5}";

        var statsPanel = new Panel(new Markup(statsContent)).Header("[bold yellow]Player Stats[/]").Expand();
        
        var leftHandItem = game.Player.Equipment.LeftHand?.Name ?? "[grey]Empty[/]";
        var rightHandItem = game.Player.Equipment.RightHand?.Name ?? "[grey]Empty[/]";

        if (game.Player.Equipment.LeftHand != null && game.Player.Equipment.LeftHand == game.Player.Equipment.RightHand)
        {
            leftHandItem = $"[magenta]{game.Player.Equipment.LeftHand.Name} (Two-Handed)[/]";
            rightHandItem = leftHandItem;
        }

        var eqContent = $"[bold]Left Hand (L):[/]  {leftHandItem}\n[bold]Right Hand (R):[/] {rightHandItem}";
        var eqPanel = new Panel(new Markup(eqContent)).Header("[bold blue]Equipment[/]").Expand();
        
        var invBuilder = new StringBuilder();
        var items = game.Player.Inventory.GetItems();

        for (var i = 0; i < items.Length; i++)
        {
            var itemName = items[i]?.Name ?? "---";
            var slotText = $"[[{i}]] {Markup.Escape(itemName)}";

            if (i == game.Player.Inventory.SelectedIndex)
            {
                if (items[i] == null)
                    invBuilder.AppendLine($"[green]> {slotText} <[/]");
                else
                    invBuilder.AppendLine($"[green]> {slotText} <[/]");
            }
            else
            {
                if (items[i] == null)
                    invBuilder.AppendLine($"  [grey]{slotText}[/]");
                else
                    invBuilder.AppendLine($"  {slotText}");
            }
        }
        var invPanel = new Panel(new Markup(invBuilder.ToString())).Header("[bold green]Inventory[/]").Expand();
        
        var groundBuilder = new StringBuilder();
        var tileItems = game.Map.GetTile(game.Player.X, game.Player.Y).GetItems();
        
        const int maxGroundLines = 5; 

        if (tileItems.Count > 0)
        {
            for (var i = 0; i < maxGroundLines; i++)
            {
                if (i < tileItems.Count)
                {
                    if (i == maxGroundLines - 1 && tileItems.Count > maxGroundLines)
                    {
                        groundBuilder.AppendLine($"[grey]...and {tileItems.Count - i} more items[/]");
                    }
                    else
                    {
                        groundBuilder.AppendLine($"- [gold1]{Markup.Escape(tileItems[i].Name)}[/]");
                    }
                }
                else
                {
                    groundBuilder.AppendLine(); 
                }
            }
        }
        else
        {
            groundBuilder.AppendLine("[grey]No items here.[/]");
            for (var i = 1; i < maxGroundLines; i++) groundBuilder.AppendLine();
        }
        var groundPanel = new Panel(new Markup(groundBuilder.ToString())).Header("[bold]Ground[/]").Expand();
        
        var rightColumn = new Rows(statsPanel, eqPanel, invPanel, groundPanel);
        layout.AddRow(leftColumn, rightColumn);

        AnsiConsole.Write(layout);
    }
}