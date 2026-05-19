using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public class EnemyComponent : IUIComponent
{
    public string Name => "Enemy";

    public IRenderable? Build(IClientModel model)
    {
        string? enemyName;
        int currentHp, maxHp, attack, armor;

        if (model.LastState != null)
        {
            var playerTile = model.LastState.ActiveTiles.FirstOrDefault(t => t.X == model.Player.X && t.Y == model.Player.Y);
            if (playerTile == null || string.IsNullOrEmpty(playerTile.EnemyName)) return null;

            enemyName = playerTile.EnemyName;
            playerTile.EnemyStats.TryGetValue("Health", out currentHp);
            playerTile.EnemyStats.TryGetValue("MaxHealth", out maxHp);
            playerTile.EnemyStats.TryGetValue("Strength", out attack);
            playerTile.EnemyStats.TryGetValue("Armor", out armor);
        }
        else
        {
            var p = model.Player;
            var enemy = model.MapContext.Map.GetTile(p.X, p.Y).Enemy;

            if (enemy == null) return null;

            enemyName = enemy.Name;
            currentHp = enemy.Stats.GetStat(StatType.Health).Value;
            maxHp = enemy.Stats.GetStat(StatType.MaxHealth).Value;
            attack = enemy.Stats.GetStat(StatType.Strength).Value;
            armor = enemy.Stats.GetStat(StatType.Armor).Value;
        }

        var content = $"[red]Name:[/] {Markup.Escape(enemyName)}\n" +
                         $"[red]HP:[/] {currentHp} / {maxHp}\n" +
                         $"[yellow]Attack:[/] {attack}\n" +
                         $"[blue]Armor:[/] {armor}";

        return new Panel(content)
            .Header("[red]Enemy[/]")
            .BorderColor(Color.Red)
            .Expand();
    }
}