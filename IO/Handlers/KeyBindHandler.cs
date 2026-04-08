using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.IO.Handlers;

public class KeyBindHandler : InputHandlerBase
{
    private readonly ConsoleKey _handledKey;
    private readonly ICommand _command;

    public KeyBindHandler(ActionInfo actionInfo, List<ActionInfo> instructions)
    {
        _handledKey = actionInfo.Key;
        _command = actionInfo.Command;
        instructions.Add(actionInfo);
    }
    
    public override ICommand Handle(ConsoleKey key)
    {
        return key == _handledKey ? _command : base.Handle(key);
    }
}