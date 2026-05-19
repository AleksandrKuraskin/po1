using System;
using System.Collections.Generic;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.Controller.Commands;

namespace ConsoleRpg.Client.Controller.Handlers;

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