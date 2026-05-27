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
                    mapBuilder.Append("[blue]¶[/]");
                else if (tiles.TryGetValue((x, y), out var tile))
                {
                    var symbol = tile.Symbol;
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