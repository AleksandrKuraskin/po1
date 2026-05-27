using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleRpg.Client.View.Components;
using Spectre.Console;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Network.Dtos;
using ConsoleRpg.Shared.Systems.Stats;

namespace ConsoleRpg.Client.View;

public class PlayersMenuRenderer : IRenderer
{
    public void Render(IClientModel model)
    {
        if (model.LastState == null) return;

        var currentTile = model.MapContext.Map.GetTile(model.Player.X, model.Player.Y);
        var playersOnTile = new List<Player>();
        foreach (var p in currentTile.Players)
        {
            if (p.Name != model.Player.Name) playersOnTile.Add(p);
        }

        var tileGrid = new Grid().AddColumns(3);
        for (var i = 0; i < playersOnTile.Count; i += 3)
        {
            var cards = playersOnTile.Skip(i).Take(3).Select(CreatePlayerCard).ToArray();
            tileGrid.AddRow(cards);
        }

        var allOtherPlayers = model.LastState.OtherPlayers;
        var allGrid = new Grid().AddColumns(3);
        for (var i = 0; i < allOtherPlayers.Count; i += 3)
        {
            var cards = allOtherPlayers.Skip(i).Take(3).Select(CreatePlayerCardDto).ToArray();
            allGrid.AddRow(cards);
        }

        var layout = new Layout("Root")
            .SplitColumns(
                new Layout("Content").Ratio(4),
                new Layout("Controls").Ratio(1)
            );

        layout["Content"].SplitRows(
            new Layout("TilePlayers").Ratio(1),
            new Layout("AllPlayers").Ratio(1)
        );

        layout["Content"]["TilePlayers"].Update(new Panel(tileGrid).Header("[bold yellow]Players on this Tile[/]").Expand());
        layout["Content"]["AllPlayers"].Update(new Panel(allGrid).Header("[bold green]Other Players Online[/]").Expand());
        layout["Controls"].Update(new ControlsComponent().Build(model));

        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }

    private string FormatStatValue(int total, int baseValue)
    {
        var bonus = total - baseValue;
        return bonus switch
        {
            > 0 => $"[yellow]{total,-4}[/] ([grey]{baseValue}[/] + [green]{bonus}[/])",
            < 0 => $"[grey]{total,-4}[/] ([grey]{baseValue}[/] - [red]{Math.Abs(bonus)}[/])",
            _ => $"{total,-4}"
        };
    }

    private Panel CreatePlayerCard(Player p)
    {
        var stats = p.Stats;
        string GetStatFormatted(StatType type) {
            var s = stats.GetStat(type);
            return FormatStatValue(s.Value, s.BaseValue);
        }

        var maxHealth = stats.GetStat(StatType.MaxHealth).Value;
        var currentHealth = stats.GetStat(StatType.Health).Value;

        var statsContent = $@"
        [bold]Health:[/]        [green]{currentHealth,-4}[/]/[green] {maxHealth}[/]
        [bold]Armor:[/]         {GetStatFormatted(StatType.Armor)}
        [bold]Strength:[/]      {GetStatFormatted(StatType.Strength)}
        [bold]Aggression:[/]    {GetStatFormatted(StatType.Aggression)}
        [bold]Intelligence:[/]  {GetStatFormatted(StatType.Intelligence)}
        [bold]Agility:[/]       {GetStatFormatted(StatType.Agility)}
        [bold]Luck:[/]          {GetStatFormatted(StatType.Luck)}

        [bold gold1]Gold:[/] {p.Wallet.GoldValue,-5} | [bold silver]Coins:[/] {p.Wallet.CoinValue,-5}";

        var equipContent = string.Join("\n", p.Equipment.GetAllEquipped().Select(e => $"[yellow]{e.Key}:[/] {UIStyleRegistry.FormatItem(e.Value.GetState())}"));
        
        return new Panel(new Rows(
            new Markup($"[bold cyan]{Markup.Escape(p.Name)}[/]"),
            new Rule(),
            new Markup(statsContent),
            new Rule("[italic]Equipment[/]"),
            new Markup(equipContent)
        ))
        {
            Width = 400,
        }.RoundedBorder();
    }

    private Panel CreatePlayerCardDto(PlayerDto p)
    {
        var stats = p.Stats;
        string GetStatFormatted(StatType type) {
            if (!stats.TryGetValue(type, out var s)) return "0";
            return FormatStatValue(s.Value, s.BaseValue);
        }

        var maxHealth = stats.TryGetValue(StatType.MaxHealth, out var mh) ? mh.Value : 100;
        var currentHealth = stats.TryGetValue(StatType.Health, out var ch) ? ch.Value : 100;

        var statsContent = $@"
        [bold]Health:[/]        [green]{currentHealth,-4}[/]/[green] {maxHealth}[/]
        [bold]Armor:[/]         {GetStatFormatted(StatType.Armor)}
        [bold]Strength:[/]      {GetStatFormatted(StatType.Strength)}
        [bold]Aggression:[/]    {GetStatFormatted(StatType.Aggression)}
        [bold]Intelligence:[/]  {GetStatFormatted(StatType.Intelligence)}
        [bold]Agility:[/]       {GetStatFormatted(StatType.Agility)}
        [bold]Luck:[/]          {GetStatFormatted(StatType.Luck)}

        [bold gold1]Gold:[/] {p.Gold,-5} | [bold silver]Coins:[/] {p.Coins,-5}";

        var equipContent = string.Join("\n", p.Equipment.Select(e => $"[yellow]{e.Key}:[/] {UIStyleRegistry.FormatItem(e.Value)}"));

        return new Panel(new Rows(
            new Markup($"[bold cyan]{Markup.Escape(p.Name)}[/]"),
            new Rule(),
            new Markup(statsContent),
            new Rule("[italic]Equipment[/]"),
            new Markup(equipContent)
        ))
        {
            Width = 400,
        }.RoundedBorder();
    }

    public void AddSidebarComponent(IUIComponent component) { }
    public void ClearSidebarComponents() { }
}
