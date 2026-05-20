using System.Text.Json;
using ConsoleRpg.Shared.Entities;

namespace ConsoleRpg.Server.CommandHandlers;

public class EquipCommandHandler : IServerCommandHandler
{
    public string CommandName => "EQUIP";

    public void Handle(string payload, IServerModel server, Player player)
    {
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var options = JsonSerializer.Deserialize<EquipOptions>(payload, jsonOptions);
        if (options == null) return;

        player.Inventory.SelectedIndex = options.selectedIndex;
        var item = player.Inventory.GetItemAt(options.selectedIndex);
        
        if (item != null)
        {
            item.TryEquip(player, item, options.isLeftHand);
        }
        else
        {
            player.Equipment.EquipOneHanded(player, null, options.isLeftHand);
        }
    }

    private class EquipOptions
    {
        public bool isLeftHand { get; set; }
        public int selectedIndex { get; set; }
    }
}
