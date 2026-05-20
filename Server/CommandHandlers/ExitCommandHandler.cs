using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class ExitCommandHandler : IServerCommandHandler
{
    public string CommandName => "EXIT";

    public void Handle(string payload, IServerModel server, Player player)
    {
    }
}
