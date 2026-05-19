using ConsoleRpg.Shared.Core;

namespace ConsoleRpg.Client.Controller.Commands;

public interface IServerCommand : ICommand
{
    void ExecuteServer(IClientModel model);
    
    void ICommand.Execute(IClientModel model) => ExecuteServer(model);
}
