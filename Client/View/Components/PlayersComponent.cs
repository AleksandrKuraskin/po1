using System.Linq;
using ConsoleRpg.Shared.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class PlayersComponent : IUIComponent
{
    public string Name => "Players";

    public IRenderable? Build(IClientModel model)
    {
        if (model.LastState == null) return null;

        var playerTile = model.LastState.ActiveTiles.FirstOrDefault(t => t.X == model.Player.X && t.Y == model.Player.Y);
        if (playerTile == null || playerTile.PlayerNames.Count <= 1) return null;

        var otherNames = playerTile.PlayerNames.Where(n => n != model.Player.Name).ToList();
        
        if (otherNames.Count == 0) return null;

        var text = string.Join(", ", otherNames.Select(n => $"[bold cyan]{Markup.Escape(n)}[/]"));
        
        return new Panel(new Markup(text))
            .Header("[bold yellow]Players Here[/]")
            .RoundedBorder()
            .Expand();
    }
}
