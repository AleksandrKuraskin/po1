using System;
using ConsoleRpg.Controller.Commands;

namespace ConsoleRpg.Controller.Handlers;

public class NullBind : InputHandlerBase
{
    public override ICommand Handle(ConsoleKey key)
    {
        return base.Handle(key);
    }
}