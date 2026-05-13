using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleRpg.Core.Map.Themes;

public class ThemeProvider
{
    private static readonly Random _rng = new();
    
    private static readonly List<IThemeFactory> _themes = new()
    {
        new FacultyTheme(),
        new BasementTheme(),
        new ChessboardTheme(),
        new DarkForestTheme(),
        new VoidTheme(),
    };

    public static IThemeFactory GetTheme(string themeId)
    {
        var theme = _themes.FirstOrDefault(t => t.ThemeId.Equals(themeId, StringComparison.OrdinalIgnoreCase));
        return theme ?? _themes[0]; 
    }

    public static IThemeFactory GetRandomTheme()
    {
        var theme = _themes[_rng.Next(_themes.Count)];
        return theme;
    }
}