using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class ExitCommandHandler : IServerCommandHandler
{
    public string CommandName => "EXIT";

    public void Handle(string payload, IServerModel server, Player player)
    {
        // On server, "exit" just means the player disconnected. 
        // The HandleClient finally block handles cleanup, so we don't need much here.
        // We could log it if we wanted.
    }
}
