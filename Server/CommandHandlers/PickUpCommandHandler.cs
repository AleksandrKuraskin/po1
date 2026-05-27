using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;

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
            player.MakeNoise(new PickUpSound(player, item));
            server.MapContext.Map.MarkDirty(player.X, player.Y);
        }
    }
}
