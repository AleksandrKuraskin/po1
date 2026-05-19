using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.View.Components;

namespace ConsoleRpg.Client.View;

public interface IRenderer
{
    void Render(IClientModel model);
    void AddSidebarComponent(IUIComponent component);
    void ClearSidebarComponents();
}
