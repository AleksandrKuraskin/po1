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
            dropItem = item.TryEquip(eq, inv, _isLeftHand, game.Logger);
        }
        else
        {
            dropItem = eq.EquipOneHanded(inv, null, _isLeftHand, game.Logger);
        }

        if (dropItem != null)
        {
            game.MapContext.Map.GetTile(game.Player.X, game.Player.Y).AddItem(dropItem);
        }
    }
}