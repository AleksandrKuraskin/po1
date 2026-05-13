using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Controller.Commands;

public class NullCommand: ICommand
{
    public void Execute(Game game)
    {}
}