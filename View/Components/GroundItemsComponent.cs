using System.Text;
using ConsoleRpg.Model.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Components;

public class GroundItemsComponent : IUIComponent
{
    public string Name => "Ground";
    public IRenderable? Build(Game game)
    {
        var groundBuilder = new StringBuilder();
        var tileItems = game.MapContext.Map.GetTile(game.Player.X, game.Player.Y).GetItems();
        
        const int maxGroundLines = 5; 

        if (tileItems.Count > 0)
        {
            for (var i = 0; i < maxGroundLines; i++)
            {
                if (i < tileItems.Count)
                {
                    if (i == maxGroundLines - 1 && tileItems.Count > maxGroundLines)
                    {
                        var remainingItems = tileItems.Count - i;
                        groundBuilder.AppendLine($"[grey]...and {remainingItems} more item{(remainingItems == 1 ? ' ' : 's')}[/]");
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
            return null;
        }
        return new Panel(new Markup(groundBuilder.ToString())).Header("[bold]Ground[/]").RoundedBorder().Expand();
    }
}