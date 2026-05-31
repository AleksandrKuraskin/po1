using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Client.Controller.Commands;

public class ExitGameCommand : ILocalCommand, IServerCommand
{
    public void ExecuteLocal(IClientModel model)
    {
        model.Exit();
    }

    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("EXIT", ""));
    }

    public void Execute(IClientModel model)
    {
        ExecuteLocal(model);
        ExecuteServer(model);
    }
}
