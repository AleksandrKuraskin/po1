using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Server;

public class CommandDispatcher
{
    private readonly Dictionary<string, IServerCommandHandler> _handlers = new();

    public void RegisterHandler(IServerCommandHandler handler)
    {
        _handlers[handler.CommandName] = handler;
    }

    public void Dispatch(NetworkMessage message, IServerModel server, Player player)
    {
        if (_handlers.TryGetValue(message.CommandName, out var handler))
        {
            handler.Handle(message.Payload, server, player);
        }
        else
        {
            LogManager.Instance.Log($"Server received unknown command: {message.CommandName}", LogType.Warning);
        }
    }
}