using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Client.Controller.Commands;

public class PickUpCommand : IServerCommand
{
    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("PICKUP", ""));
    }
}
