using System;
using ConsoleRpg.Client.Controller.Commands;

namespace ConsoleRpg.Client.Controller.Handlers;

public class NumericBindHandler : InputHandlerBase
{
    public override ICommand Handle(ConsoleKey key)
    {
        if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
        {
            var slotIndex = key == ConsoleKey.D0 ? 9 : key - ConsoleKey.D1;
            return new NavigateInventoryCommand(slotIndex);
        }
        
        return base.Handle(key);
    }
}