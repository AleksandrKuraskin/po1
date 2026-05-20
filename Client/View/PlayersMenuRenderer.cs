using ConsoleRpg.Client.View.Components;
using Spectre.Console;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Network;

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

    private Panel CreatePlayerCard(Player p)
    {
        var statsContent = string.Join("\n", p.Stats.GetActiveStatTypes().Select(s => $"[grey]{s}:[/] {p.Stats.GetStat(s).Value}"));
        var equipContent = string.Join("\n", p.Equipment.GetAllEquipped().Select(e => $"[yellow]{e.Key}:[/] {e.Value.Name}"));
        
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
        var statsContent = string.Join("\n", p.Stats.Select(s => $"[grey]{s.Key}:[/] {s.Value.Value}"));
        var equipContent = string.Join("\n", p.Equipment.Select(e => $"[yellow]{e.Key}:[/] {e.Value}"));

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
