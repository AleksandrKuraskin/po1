namespace ConsoleRpg.Client.Controller.Commands;

public interface ICommand
{
    void Execute(IClientModel model);
}
