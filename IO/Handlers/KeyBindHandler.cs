using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.IO.Handlers;

public class KeyBindHandler(ConsoleKey key, ICommand command) : InputHandlerBase
{
    private readonly ConsoleKey _handledKey = key;
    private readonly ICommand _command = command;
    
    public override ICommand Handle(ConsoleKey key)
    {
        return key == _handledKey ? _command : base.Handle(key);
    }
}