using ConsoleRpg.Core;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class StatsComponent : IUIComponent
{
    public IRenderable Build(Game game)
    {
        var stats = game.Player.Stats;
        var baseDamage = stats.Strength.Value;
        var eqDamage = game.Player.Equipment.GetTotalDamage();
        var totalDamage = baseDamage + eqDamage;
        
        var innerText = $@"
        [bold]Health:[/]    [green]{game.Player.Stats.MaxHealth?.Value ?? 100,-4}[/]/[green] 100[/]
        [bold]Damage:[/]    [red]{totalDamage,-4}[/] [grey](Base: {baseDamage} + Eq: {eqDamage})[/]
        [bold]Intelligence:[/]  [blue]{game.Player.Stats.Intelligence?.Value ?? 10,-4}[/]
        [bold]Agility:[/] [gold1]{game.Player.Stats.Agility?.Value ?? 10,-4}[/]
        [bold]Luck:[/] [yellow]{game.Player.Stats.Luck?.Value ?? 10,-4}[/]

        [bold gold1]Gold:[/] {game.Player.Wallet.GoldValue,-5} | [bold silver]Coins:[/] {game.Player.Wallet.CoinValue,-5}";
        
        return new Panel(new Markup(innerText)).Header("[bold yellow]Player Stats[/]").Expand();
    }
}