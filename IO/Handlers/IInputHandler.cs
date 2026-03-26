using ConsoleRpg.Core;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.IO.Handlers;

public interface IInputHandler
{
    IInputHandler SetNext(IInputHandler handler);
    ICommand Handle(ConsoleKey key);
}

public abstract class InputHandlerBase : IInputHandler
{
    private IInputHandler? _next;

    public IInputHandler SetNext(IInputHandler handler)
    {
        _next = handler;
        return _next;
    }

    public virtual ICommand Handle(ConsoleKey key)
    {
        return _next != null ? _next.Handle(key) : new LogMessageCommand("Invalid key: " + key, LogType.Error);
    }
}