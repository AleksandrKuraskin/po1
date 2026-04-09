using ConsoleRpg.Core;
using ConsoleRpg.Systems.Stats;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public class EnemyComponent : IUIComponent
{
    public string Name => "Enemy";

    public IRenderable? Build(Game game)
    {
        var p = game.Player;
        var enemy = game.MapContext.Map.GetTile(p.X, p.Y).GetEnemy();

        if (enemy == null)
        {
            return null;
        }

        var currentHp = enemy.Stats.GetStat(StatType.Health).Value;
        var maxHp = enemy.Stats.GetStat(StatType.MaxHealth).Value;
        var attack = enemy.Stats.GetStat(StatType.Strength).Value;
        var armor = enemy.Stats.GetStat(StatType.Armor).Value;

        var content = $"[red]Name:[/] {enemy.Name}\n" +
                         $"[red]HP:[/] {currentHp} / {maxHp}\n" +
                         $"[yellow]Attack:[/] {attack}\n" +
                         $"[blue]Armor:[/] {armor}";

        return new Panel(content)
            .Header("[red]Enemy[/]")
            .BorderColor(Color.Red)
            .Expand();
    }
}