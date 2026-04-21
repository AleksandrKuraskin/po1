namespace ConsoleRpg.Core.Map.Themes;

public interface IThemeFactory
{
    public string ThemeId { get; }
    public string IntroMessage { get; }
    
}