using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Client.Controller.Commands;

public class SwapHandsCommand : IServerCommand
{
    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("SWAP", ""));
    }
}
