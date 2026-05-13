using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Logging;

namespace ConsoleRpg.Controller.Commands;

public class SwapHandsCommand: ICommand
{
    public void Execute(Game game)
    {
        game.Player.Equipment.SwapHands();
        LogManager.Instance.Log("Swapped items between hands.");
    }
}