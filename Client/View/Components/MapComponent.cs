using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class MapComponent : IUIComponent
{
    
    public string Name => "Map";
    public IRenderable Build(IClientModel model)
    {
        var mapBuilder = new StringBuilder();
        var tiles = model.LocalActiveTiles;
        
        var width = model.MapContext.Map.Width;
        var height = model.MapContext.Map.Height;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (model.Player.X == x && model.Player.Y == y)
                    mapBuilder.Append($"[cyan]{model.Player.Symbol}[/]");
                else if (tiles.TryGetValue((x, y), out var tile))
                {
                    if (tile.PlayerNames.Count > 0)
                        mapBuilder.Append($"[cyan]{tile.Symbol}[/]");
                    else if (tile.EnemyName != null)
                        mapBuilder.Append($"[red]{tile.Symbol}[/]");
                    else if (tile.Items.Count > 0)
                    {
                        var hasWeapon = tile.Items.Any(i => i.Symbol == 'w');
                        var color = hasWeapon ? "yellow" : "white";
                        mapBuilder.Append($"[{color}]{tile.Symbol}[/]");
                    }
                    else if (tile.IsWall)
                        mapBuilder.Append("[grey]█[/]");
                    else
                        mapBuilder.Append(" ");
                }
                else
                {
                    mapBuilder.Append("[grey]█[/]");
                }
            }
            mapBuilder.AppendLine();
        }
        return new Panel(new Markup(mapBuilder.ToString())).Header("[bold cyan]Dungeon Map[/]").RoundedBorder().Expand();
    }
}