using ConsoleRpg.Core;
using ConsoleRpg.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class StatsComponent : IUIComponent
{
    public string Name => "Stats";
    
    public IRenderable Build(Game game)
    {
        var stats = game.Player.Stats;
        var baseDamage = stats.GetStat(StatType.Strength).Value;
        var eqDamage = game.Player.Equipment.GetTotalDamage();
        var totalDamage = baseDamage + eqDamage;
        
        var baseHealth = stats.GetStat(StatType.MaxHealth).Value;
        var currentHealth = stats.GetStat(StatType.Health).Value;
        
        var armor = stats.GetStat(StatType.Armor).Value;
        
        var aggression = stats.GetStat(StatType.Aggression).Value;
        var agility = stats.GetStat(StatType.Agility).Value;
        var intelligence = stats.GetStat(StatType.Intelligence).Value;
        var luck = stats.GetStat(StatType.Luck).Value;
        
        var innerText = $@"
        [bold]Health:[/]    [green]{currentHealth,-4}[/]/[green] {baseHealth}[/]
        [bold]Armor:[/]    [grey]{armor,-4}[/]
        [bold]Damage:[/]    [red]{totalDamage,-4}[/]([green]+{eqDamage}[/])
        [bold]Aggression:[/]    [red]{aggression,-4}[/]
        [bold]Intelligence:[/]  [blue]{intelligence,-4}[/]
        [bold]Agility:[/] [gold1]{agility,-4}[/]
        [bold]Luck:[/] [yellow]{luck,-4}[/]

        [bold gold1]Gold:[/] {game.Player.Wallet.GoldValue,-5} | [bold silver]Coins:[/] {game.Player.Wallet.CoinValue,-5}";
        
        return new Panel(new Markup(innerText)).Header("[bold yellow]Player Stats[/]").RoundedBorder().Expand();
    }
}