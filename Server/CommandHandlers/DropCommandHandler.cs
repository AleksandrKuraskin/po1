using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class DropCommandHandler : IServerCommandHandler
{
    public string CommandName => "DROP";

    public void Handle(string payload, IServerModel server, Player player)
    {
        if (!int.TryParse(payload, out var index))
        {
            index = player.Inventory.SelectedIndex;
        }

        var item = player.Inventory.RemoveItemAt(index);
        if (item != null)
        {
            item.OnDrop(player, server.MapContext.Map, item);
            server.MapContext.Map.MarkDirty(player.X, player.Y);
        }
    }
}
