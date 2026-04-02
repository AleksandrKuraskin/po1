using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public interface IInputState
{
    public string Name { get; }
    public List<ActionInfo> Instructions { get; }

    string GetInstructions();
    
    IInputState GetNewState(Game game);
    ICommand HandleInput(ConsoleKey key);
}