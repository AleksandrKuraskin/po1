using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class SwapCommandHandler : IServerCommandHandler
{
    public string CommandName => "SWAP";

    public void Handle(string payload, IServerModel server, Player player)
    {
        player.Equipment.SwapHands();
    }
}
