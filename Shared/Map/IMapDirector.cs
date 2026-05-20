using ConsoleRpg.Shared.Map.Themes;

namespace ConsoleRpg.Shared.Map;

public interface IMapDirector
{
    public void ConstructRandom(IThemeFactory theme);
    public void ConstructRoom(IThemeFactory theme);
    public void ConstructEmpty(IThemeFactory theme);
}