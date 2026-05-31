using ConsoleRpg.Shared.Core;
using Spectre.Console.Rendering;

namespace ConsoleRpg.Client.View.Components;

public interface IUIComponent
{
    string Name { get; }
    IRenderable? Build(IClientModel model);
}