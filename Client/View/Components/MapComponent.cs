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
        
        if (model.LastState != null)
        {
            var tiles = model.LastState.ActiveTiles.ToDictionary(t => (t.X, t.Y));
            var width = model.LastState.ActiveTiles.Max(t => t.X) + 1;
            var height = model.LastState.ActiveTiles.Max(t => t.Y) + 1;
            
            width = Math.Max(width, model.MapContext.Map.Width);
            height = Math.Max(height, model.MapContext.Map.Height);

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
        }
        else
        {
            for (var y = 0; y < model.MapContext.Map.Height; y++)
            {
                for (var x = 0; x < model.MapContext.Map.Width; x++)
                {
                    if (model.Player.X == x && model.Player.Y == y)
                        mapBuilder.Append("[blue]¶[/]");
                    else
                    {
                        var symbol = model.MapContext.Map.GetTile(x, y).GetSymbol();
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
        }
        return new Panel(new Markup(mapBuilder.ToString())).Header("[bold cyan]Dungeon Map[/]").RoundedBorder().Expand();
    }
}