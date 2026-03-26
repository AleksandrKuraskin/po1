using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.IO.Handlers;

public class NumericBindHandler : InputHandlerBase
{
    public override ICommand Handle(ConsoleKey key)
    {
        if (key >= ConsoleKey.D0 && key <= ConsoleKey.D9)
        {
            var slotIndex = key == ConsoleKey.D0 ? 9 : key - ConsoleKey.D1;
            Console.WriteLine(slotIndex);
            
            return new NavigateInventoryCommand(slotIndex);
        }
        
        return base.Handle(key);
    }
}