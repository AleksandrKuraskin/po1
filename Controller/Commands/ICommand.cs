using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Controller.Commands;

public interface ICommand
{
    void Execute(Game game);
}