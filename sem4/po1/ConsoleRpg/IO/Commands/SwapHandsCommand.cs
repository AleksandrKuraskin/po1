using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Commands;

public class SwapHandsCommand: ICommand
{
    public void Execute(Game game)
    {
        game.Player.Equipment.SwapHands();
        game.Logger.Log("Swapped items between hands.");
    }
}