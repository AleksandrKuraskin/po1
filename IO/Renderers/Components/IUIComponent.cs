using ConsoleRpg.Core;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public interface IUIComponent
{
    IRenderable Build(Game game);
}