using System.Text.Json;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class JoinCommandHandler : IServerCommandHandler
{
    public string CommandName => "JOIN";

    public void Handle(string payload, IServerModel server, Player player)
    {
        // Special case: player is created during connection, 
        // but this could handle setting the name or other join logic.
        player.Name = payload;
        
        if (server.MapContext.SoundMediator != null)
        {
            player.SetMediator(server.MapContext.SoundMediator);
        }

        var (x, y) = server.MapContext.Map.GetRandomFreeTile();
        player.SetPosition(x, y);
        server.MapContext.Map.SpawnPlayer(player);
    }
}
