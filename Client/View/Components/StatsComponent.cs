using System;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class StatsComponent : IUIComponent
{
    public string Name => "Stats";
    
    public IRenderable Build(IClientModel model)
    {
        if (model.LastState == null) return new Panel("Loading...").Header("[bold yellow]Player Stats[/]");
        var statsDto = model.LastState.LocalPlayer.Stats;
        
        
        string FormatStat(string statName)
        {
            if (!statsDto.TryGetValue(statName, out var stat)) return "0";
            
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
       
        var maxHealth = statsDto[nameof(StatType.MaxHealth)].Value;
        var currentHealth = statsDto[nameof(StatType.Health)].Value;
        
        var innerText = $@"
        [bold]Health:[/]        [green]{currentHealth,-4}[/]/[green] {maxHealth}[/]
        [bold]Armor:[/]         {FormatStat(nameof(StatType.Armor))}
        [bold]Strength:[/]      {FormatStat(nameof(StatType.Strength))}
        [bold]Aggression:[/]    {FormatStat(nameof(StatType.Aggression))}
        [bold]Intelligence:[/]  {FormatStat(nameof(StatType.Intelligence))}
        [bold]Agility:[/]       {FormatStat(nameof(StatType.Agility))}
        [bold]Luck:[/]          {FormatStat(nameof(StatType.Luck))}

        [bold gold1]Gold:[/] {model.LastState.LocalPlayer.Gold,-5} | [bold silver]Coins:[/] {model.LastState.LocalPlayer.Coins,-5}";
        
        return new Panel(new Markup(innerText)).Header("[bold yellow]Player Stats[/]").RoundedBorder().Expand();
    }
}