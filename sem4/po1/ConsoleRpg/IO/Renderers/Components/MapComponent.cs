using System.Text;
using ConsoleRpg.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class MapComponent : IUIComponent
{
    public IRenderable Build(Game game)
    {
        var mapBuilder = new StringBuilder();
        for (var y = 0; y < game.MapContext.Map.Height; y++)
        {
            for (var x = 0; x < game.MapContext.Map.Width; x++)
            {
                if (game.Player.X == x && game.Player.Y == y)
                    mapBuilder.Append("[blue]¶[/]");
                else
                {
                    var symbol = game.MapContext.Map.GetTile(x, y).GetSymbol();
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
        return new Panel(new Markup(mapBuilder.ToString())).Header("[bold cyan]Dungeon Map[/]").Expand();
    }
}