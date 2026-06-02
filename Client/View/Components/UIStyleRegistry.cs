using System.Collections.Generic;
using ConsoleRpg.Shared.Systems.Network.Dtos;

namespace ConsoleRpg.Client.View.Components;

public class ItemUIStyle
{
    public string TextColor { get; set; } = "white";
}

public interface IItemStyleStrategy { void Apply(ItemUIStyle style); }

public class GodlyStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "gold1"; } }
public class StrongStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "red"; } }
public class UnluckyStyleStrategy : IItemStyleStrategy { public void Apply(ItemUIStyle s) { s.TextColor = "grey"; } }

public static class UIStyleRegistry
{
    private static readonly Dictionary<string, IItemStyleStrategy> _styles = new()
    {
        { "godly", new GodlyStyleStrategy() },
        { "strong", new StrongStyleStrategy() },
        { "unlucky", new UnluckyStyleStrategy() }
    };

    public static bool AreItemsEqual(ItemDto? a, ItemDto? b)
    {
        if (a == b) return true;
        if (a == null || b == null) return false;
        if (a.Name != b.Name || a.DecoratorId != b.DecoratorId) return false;
        return AreItemsEqual(a.Wrappee, b.Wrappee);
    }

    public static string FormatItem(ItemDto item)
    {
        var style = new ItemUIStyle();
        var current = item;
        string? itemName = null;

        while (current != null)
        {
            if (current.DecoratorId != null)
            {
                if (_styles.TryGetValue(current.DecoratorId, out var strategy)) strategy.Apply(style);
            }
            
            if (current.Name != null && itemName == null) itemName = current.Name;
            
            current = current.Wrappee;
        }
        
        return $"[{style.TextColor}]{itemName ?? "Unknown"}[/]";
    }
}
