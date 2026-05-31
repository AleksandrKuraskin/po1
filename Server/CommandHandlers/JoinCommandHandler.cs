using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class JoinCommandHandler : IServerCommandHandler
{
    public string CommandName => "JOIN";

    public void Handle(string payload, IServerModel server, Player player)
    {
        var name = payload;
        var suffix = 1;
        while (server.GetAllPlayers().Any(p => p != player && p.Name == name))
        {
            name = $"{payload}{suffix++}";
        }
        player.Name = name;
        
        if (server.MapContext.SoundMediator != null)
        {
            player.SetMediator(server.MapContext.SoundMediator);
        }

        var (x, y) = server.MapContext.Map.GetRandomFreeTile();
        player.SetPosition(x, y);
        server.MapContext.Map.SpawnPlayer(player);
    }
}
