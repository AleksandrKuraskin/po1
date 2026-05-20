namespace ConsoleRpg.Client.Controller.Commands;

public class NavigateInventoryCommand(int index) : ILocalCommand
{
    public void ExecuteLocal(IClientModel model)
    {
        var inv = model.Player.Inventory;
        if (index < 0)
        {
            var direction = index == -1 ? -1 : 1;
            inv.SelectedIndex = Math.Clamp(inv.SelectedIndex + direction, 0, inv.Capacity - 1);
        }
        else
        {
            inv.SelectedIndex = index;
        }
    }
}
