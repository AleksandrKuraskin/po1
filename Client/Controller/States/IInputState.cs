using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.Controller.Commands;

namespace ConsoleRpg.Client.Controller.States;

public interface IInputState
{
    public string Name { get; }
    public List<ActionInfo> Instructions { get; }

    string GetInstructions();
    
    IInputState GetNewState(IGameModel game);
    ICommand HandleInput(ConsoleKey key);
}