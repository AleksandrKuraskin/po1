using System;
using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Renderers.Components;

public class StatsComponent : IUIComponent
{
    public string Name => "Stats";
    
    public IRenderable Build(Game game)
    {
        var stats = game.Player.Stats;
        
        string FormatStat(StatType type)
        {
            var stat = stats.GetStat(type);
            var total = stat.Value;
            var baseValue = stat.BaseValue;
            var bonus = total - baseValue;

            return bonus switch
            {
                > 0 => $"[yellow]{total,-4}[/] ([grey]{baseValue}[/] + [green]{bonus}[/])",
                < 0 => $"[grey]{total,-4}[/] ([grey]{baseValue}[/] - [red]{Math.Abs(bonus)}[/])",
                _ => $"{total,-4}"
            };
        }
       
        var maxHealth = stats.GetStat(StatType.MaxHealth);
        var currentHealth = stats.GetStat(StatType.Health);
        
        var innerText = $@"
        [bold]Health:[/]        [green]{currentHealth.Value,-4}[/]/[green] {maxHealth.Value}[/]
        [bold]Armor:[/]         {FormatStat(StatType.Armor)}
        [bold]Strength:[/]      {FormatStat(StatType.Strength)}
        [bold]Aggression:[/]    {FormatStat(StatType.Aggression)}
        [bold]Intelligence:[/]  {FormatStat(StatType.Intelligence)}
        [bold]Agility:[/]       {FormatStat(StatType.Agility)}
        [bold]Luck:[/]          {FormatStat(StatType.Luck)}

        [bold gold1]Gold:[/] {game.Player.Wallet.GoldValue,-5} | [bold silver]Coins:[/] {game.Player.Wallet.CoinValue,-5}";
        
        return new Panel(new Markup(innerText)).Header("[bold yellow]Player Stats[/]").RoundedBorder().Expand();
    }
}