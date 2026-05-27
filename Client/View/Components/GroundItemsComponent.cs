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
        List<string> itemStrings;

        if (model.LastState != null)
        {
            var playerTile = model.LastState.ActiveTiles.FirstOrDefault(t => t.X == model.Player.X && t.Y == model.Player.Y);
            itemStrings = playerTile?.Items.Select(i => UIStyleRegistry.FormatItem(i)).ToList() ?? [];
        }
        else
        {
            var tileItems = model.MapContext.Map.GetTile(model.Player.X, model.Player.Y).GetItems();
            itemStrings = tileItems.Select(i => UIStyleRegistry.FormatItem(i.GetState())).ToList();
        }
        
        const int maxGroundLines = 5; 

        if (itemStrings.Count > 0)
        {
            for (var i = 0; i < maxGroundLines; i++)
            {
                if (i < itemStrings.Count)
                {
                    if (i == maxGroundLines - 1 && itemStrings.Count > maxGroundLines)
                    {
                        var remainingItems = itemStrings.Count - i;
                        groundBuilder.AppendLine($"[grey]...and {remainingItems} more item{(remainingItems == 1 ? "" : "s")}[/]");
                    }
                    else
                    {
                        groundBuilder.AppendLine($"- {itemStrings[i]}");
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