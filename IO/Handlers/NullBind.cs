using System;
using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.IO.Handlers;

public class NullBind : InputHandlerBase
{
    public override ICommand Handle(ConsoleKey key)
    {
        return base.Handle(key);
    }
}