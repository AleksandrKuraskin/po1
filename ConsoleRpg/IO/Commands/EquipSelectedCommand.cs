using ConsoleRpg.Core;
using ConsoleRpg.Items;

namespace ConsoleRpg.IO.Commands;

public class EquipSelectedCommand(bool isLeftHand) : ICommand
{
    private readonly bool _isLeftHand = isLeftHand;
    
    public void Execute(Game game)
    {
        var inv = game.Player.Inventory;
        var item = inv.GetItemAt(inv.SelectedIndex);
        var eq = game.Player.Equipment;

        IItem? dropItem;

        if (item != null)
        {
            dropItem = item.TryEquip(game.Player, item, _isLeftHand);
        }
        else
        {
            dropItem = eq.EquipOneHanded(game.Player, null, _isLeftHand);
        }

        if (dropItem != null)
        {
            game.MapContext.Map.GetTile(game.Player.X, game.Player.Y).AddItem(dropItem);
        }
    }
}