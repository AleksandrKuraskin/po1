using ConsoleRpg.Shared.Maps.Themes;

namespace ConsoleRpg.Shared.Maps;

public interface IMapDirector
{
    public void ConstructRandom(IThemeFactory theme);
    public void ConstructRoom(IThemeFactory theme);
    public void ConstructEmpty(IThemeFactory theme);
}