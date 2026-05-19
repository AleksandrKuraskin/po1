using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Client.Controller.Commands;

public class PickUpCommand : IServerCommand
{
    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("PICKUP", ""));
    }
}
