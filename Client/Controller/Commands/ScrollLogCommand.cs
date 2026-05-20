namespace ConsoleRpg.Client.Controller.Commands;

public class ScrollLogCommand(int delta) : ILocalCommand
{
    private readonly int _delta = delta;

    public void ExecuteLocal(IClientModel model)
    {
        model.Logger.ScrollOffset = Math.Max(0, model.Logger.ScrollOffset + _delta);
    }
}
