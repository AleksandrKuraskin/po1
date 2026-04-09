using ConsoleRpg.Core;
using Spectre.Console.Rendering;

namespace ConsoleRpg.IO.Renderers.Components;

public interface IUIComponent
{
    string Name { get; }
    IRenderable? Build(Game game);
}