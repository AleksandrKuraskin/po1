using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Items;
using ConsoleRpg.Shared.Systems.Network;
using System.Text.Json;

namespace ConsoleRpg.Client.Controller.Commands;

public class EquipSelectedCommand(bool isLeftHand) : IServerCommand
{
    private readonly bool _isLeftHand = isLeftHand;
    
    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("EQUIP", JsonSerializer.Serialize(new { isLeftHand = _isLeftHand, selectedIndex = model.Player.Inventory.SelectedIndex })));
    }
}
