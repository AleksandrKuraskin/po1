using ConsoleRpg.Shared.Core;

namespace ConsoleRpg.Client.Controller.Commands;

public interface ILocalCommand : ICommand
{
    void ExecuteLocal(IClientModel model);
    
    void ICommand.Execute(IClientModel model) => ExecuteLocal(model);
}
