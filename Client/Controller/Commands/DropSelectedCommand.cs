using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Client.Controller.Commands;

public class DropSelectedCommand : IServerCommand
{
    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("DROP", model.Player.Inventory.SelectedIndex.ToString()));
    }
}
