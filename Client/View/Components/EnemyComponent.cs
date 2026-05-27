using ConsoleRpg.Shared.Systems.Network.Dtos;
using ConsoleRpg.Shared.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class EnemyComponent : IUIComponent
{
    public string Name => "Enemy";

    public IRenderable? Build(IClientModel model)
    {
        string enemyName;
        StatDto? hpStat = null, maxHpStat = null, strengthStat = null, armorStat = null;

        if (model.LastState != null)
        {
            var playerTile = model.LastState.ActiveTiles.FirstOrDefault(t => t.X == model.Player.X && t.Y == model.Player.Y);
            if (playerTile == null || string.IsNullOrEmpty(playerTile.EnemyName)) return null;

            enemyName = playerTile.EnemyName;
            playerTile.EnemyStats.TryGetValue(StatType.Health, out hpStat);
            playerTile.EnemyStats.TryGetValue(StatType.MaxHealth, out maxHpStat);
            playerTile.EnemyStats.TryGetValue(StatType.Strength, out strengthStat);
            playerTile.EnemyStats.TryGetValue(StatType.Armor, out armorStat);
        }
        else
        {
            var p = model.Player;
            var enemy = model.MapContext.Map.GetTile(p.X, p.Y).Enemy;

            if (enemy == null) return null;

            enemyName = enemy.Name;
            
            hpStat = new StatDto { Value = enemy.Stats.GetStat(StatType.Health).Value, BaseValue = enemy.Stats.GetStat(StatType.Health).BaseValue };
            maxHpStat = new StatDto { Value = enemy.Stats.GetStat(StatType.MaxHealth).Value, BaseValue = enemy.Stats.GetStat(StatType.MaxHealth).BaseValue };
            strengthStat = new StatDto { Value = enemy.Stats.GetStat(StatType.Strength).Value, BaseValue = enemy.Stats.GetStat(StatType.Strength).BaseValue };
            armorStat = new StatDto { Value = enemy.Stats.GetStat(StatType.Armor).Value, BaseValue = enemy.Stats.GetStat(StatType.Armor).BaseValue };
        }

        string FormatStat(StatDto? stat)
        {
            if (stat == null) return "0";
            
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

        var content = $"[bold]Name:[/]         {Markup.Escape(enemyName)}\n" +
                      $"[bold]Health:[/]       [green]{hpStat?.Value ?? 0,-4}[/]/[green] {maxHpStat?.Value ?? 0}[/]\n" +
                      $"[bold]Strength:[/]     {FormatStat(strengthStat)}\n" +
                      $"[bold]Armor:[/]        {FormatStat(armorStat)}";

        return new Panel(content)
            .Header("[red]Enemy[/]")
            .BorderColor(Color.Red)
            .Expand();
    }
}