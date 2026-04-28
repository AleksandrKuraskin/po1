using ConsoleRpg.Core.Map.Themes;

namespace ConsoleRpg.Core.Map;

public interface IMapDirector
{
    public void ConstructRandom(IThemeFactory theme);
    public void ConstructRoom(IThemeFactory theme);
    public void ConstructEmpty(IThemeFactory theme);
}