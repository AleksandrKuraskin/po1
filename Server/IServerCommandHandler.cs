using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server;

public interface IServerCommandHandler
{
    string CommandName { get; }
    void Handle(string payload, IServerModel server, Player player);
}
