using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class NullCommand: ICommand
{
    public void Execute(Game game){}
}