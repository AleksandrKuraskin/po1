using ConsoleRpg.Core;
using ConsoleRpg.Systems.Logging;

namespace ConsoleRpg.IO.Commands;

public class SwapHandsCommand: ICommand
{
    public void Execute(Game game)
    {
        game.Player.Equipment.SwapHands();
        LogManager.Instance.Log("Swapped items between hands.");
    }
}