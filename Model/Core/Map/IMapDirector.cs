using ConsoleRpg.Model.Core.Map.Themes;

namespace ConsoleRpg.Model.Core.Map;

public interface IMapDirector
{
    public void ConstructRandom(IThemeFactory theme);
    public void ConstructRoom(IThemeFactory theme);
    public void ConstructEmpty(IThemeFactory theme);
}