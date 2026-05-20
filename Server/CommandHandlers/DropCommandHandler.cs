using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class DropCommandHandler : IServerCommandHandler
{
    public string CommandName => "DROP";

    public void Handle(string payload, IServerModel server, Player player)
    {
        var item = player.Inventory.RemoveItemAt(player.Inventory.SelectedIndex);
        if (item != null)
        {
            item.OnDrop(player, server.MapContext.Map, item);
        }
    }
}
