using ConsoleRpg.Model.Core;
using Spectre.Console.Rendering;

namespace ConsoleRpg.View.Renderers.Components;

public interface IUIComponent
{
    string Name { get; }
    IRenderable? Build(Game game);
}