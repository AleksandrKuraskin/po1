using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public interface ICommand
{
    void Execute(Game game);
}