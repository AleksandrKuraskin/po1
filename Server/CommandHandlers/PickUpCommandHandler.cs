using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class PickUpCommandHandler : IServerCommandHandler
{
    public string CommandName => "PICKUP";

    public void Handle(string payload, IServerModel server, Player player)
    {
        var tile = server.MapContext.Map.GetTile(player.X, player.Y);
        var item = tile.GetTopItem();
        if (item != null && item.TryPickUp(player, item))
        {
            tile.RemoveTopItem();
            server.MapContext.Map.MarkDirty(player.X, player.Y);
        }
    }
}
