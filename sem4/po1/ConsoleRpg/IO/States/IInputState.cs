using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public interface IInputState
{
    ICommand HandleInput(ConsoleKey key);
}