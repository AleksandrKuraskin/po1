namespace ConsoleRpg.Shared.Core;

public interface IServerCommandHandler
{
    string CommandName { get; }
    void Handle(string payload, IServerModel server, Entities.Player player);
}
