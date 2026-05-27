using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Network.Dtos;

namespace ConsoleRpg.Client.View.Components;

public class ItemUIStyle
{
    public string TextColor { get; set; } = "white";
    public string Prefix { get; set; } = "";
}

public interface IItemStyleStrategy { void Apply(ItemUIStyle style); }

public class GodlyStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "gold1"; s.Prefix = "[[\u2b50]]"; } }
public class StrongStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "red"; s.Prefix = "[[\u2694 \ufe0f]]"; } }
public class UnluckyStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "grey"; } }

public static class UIStyleRegistry
{
    private static readonly Dictionary<string, IItemStyleStrategy> _styles = new()
    {
        { "godly", new GodlyStyleStrategy() },
        { "strong", new StrongStyleStrategy() },
        { "unlucky", new UnluckyStyleStrategy() }
    };

    public static string FormatItem(ItemDto item)
    {
        var style = new ItemUIStyle();
        foreach (var tag in item.Decorators)
        {
            if (_styles.TryGetValue(tag, out var strategy)) strategy.Apply(style);
        }
        
        return $"[{style.TextColor}]{style.Prefix} {item.Name}[/]";
    }
}
