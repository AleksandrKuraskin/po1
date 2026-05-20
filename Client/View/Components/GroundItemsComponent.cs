using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class GroundItemsComponent : IUIComponent
{
    public string Name => "Ground";
    public IRenderable? Build(IClientModel model)
    {
        var groundBuilder = new StringBuilder();
        List<string> itemNames;

        if (model.LastState != null)
        {
            var playerTile = model.LastState.ActiveTiles.FirstOrDefault(t => t.X == model.Player.X && t.Y == model.Player.Y);
            itemNames = playerTile?.ItemNames ?? [];
        }
        else
        {
            var tileItems = model.MapContext.Map.GetTile(model.Player.X, model.Player.Y).GetItems();
            itemNames = tileItems.Select(i => i.Name).ToList();
        }
        
        const int maxGroundLines = 5; 

        if (itemNames.Count > 0)
        {
            for (var i = 0; i < maxGroundLines; i++)
            {
                if (i < itemNames.Count)
                {
                    if (i == maxGroundLines - 1 && itemNames.Count > maxGroundLines)
                    {
                        var remainingItems = itemNames.Count - i;
                        groundBuilder.AppendLine($"[grey]...and {remainingItems} more item{(remainingItems == 1 ? "" : "s")}[/]");
                    }
                    else
                    {
                        groundBuilder.AppendLine($"- [gold1]{Markup.Escape(itemNames[i])}[/]");
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