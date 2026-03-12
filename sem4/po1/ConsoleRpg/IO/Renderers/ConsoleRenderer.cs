using System.Text;
using Spectre.Console;
using ConsoleRpg.Core;
using ConsoleRpg.Entities;


namespace ConsoleRpg.IO.Renderers;

public class ConsoleRenderer : IRenderer
{
    private Panel BuildInvenotryPanel(Player p)
    {
        var invBuilder = new StringBuilder();
        var items = p.Inventory.GetItems();
        for (var i = 0; i < 9; i++)
            invBuilder.AppendLine($"{i + 1}: {items[i]?.Name ?? "-"}");
        invBuilder.AppendLine($"0: {items[9]?.Name ?? "-"}");

        var invPanel = new Panel(invBuilder.ToString()).Header("[green]Inventory[/]").BorderColor(Color.Green);
        return invPanel;
    }

    private Panel BuildEquipmentPanel(Player p)
    {
        var eqPanel = new Panel(
            $"[gold1]Gold:[/] {p.Wallet.GoldValue}\n[silver]Coins:[/] {p.Wallet.CoinValue}\n\n" +
            $"[blue]--- Equipment ---[/]\n" +
            $"Left hand: {p.Equipment.LeftHand?.Name ?? "Empty"}\n" +
            $"Right hand: {p.Equipment.RightHand?.Name ?? "Empty"}\n" +
            (p.Equipment.LeftHand == p.Equipment.RightHand && p.Equipment.LeftHand != null ?
                "[green](Two handed)[/]\n" :
                "")
        ).Header("[blue]Status[/]").BorderColor(Color.Blue);

        return eqPanel;
    }

    private Panel BuildMessagePanel(Game game)
    {
        var msgPanel = new Panel(
            $"[white]{game.Logger}[/]\n\n[grey]WASD-Move, E-PickUp, Q+|0-9|-Drop, |0-9|-Equip[/]")
            .Expand();
        return msgPanel;
    }
    
    public void Render(Game game)
    {
        var mapBuilder = new StringBuilder();
        var map = game.Map;

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                mapBuilder.Append(map.GetTile(x, y).GetSymbol());
            }
            mapBuilder.AppendLine();
        }

        var mapPanel = new Panel(mapBuilder.ToString()).Header("[yellow]Map[/]").BorderColor(Color.Grey);

        var p = game.Player;
        var eqPanel = BuildEquipmentPanel(p);
        var invPanel = BuildInvenotryPanel(p);

        var msgPanel = BuildMessagePanel(game);

        var rightCol = new Grid().AddColumn(new GridColumn()).AddRow(eqPanel).AddRow(invPanel);
        
        var layout = new Grid()
            .AddColumn(new GridColumn().NoWrap())
            .AddColumn(new GridColumn())
            .AddRow(mapPanel, rightCol)
            .AddRow(msgPanel);

        Console.SetCursorPosition(0, 0);
        AnsiConsole.Write(layout);
    }
}