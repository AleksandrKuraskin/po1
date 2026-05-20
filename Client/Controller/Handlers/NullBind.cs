using ConsoleRpg.Client.Controller.Commands;

namespace ConsoleRpg.Client.Controller.Handlers;

public class NullBind : InputHandlerBase
{
    public override ICommand Handle(ConsoleKey key)
    {
        return base.Handle(key);
    }
}