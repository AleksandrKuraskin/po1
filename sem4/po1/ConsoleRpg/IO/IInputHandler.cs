using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public interface IInputHandler
{
    void SetNext(IInputHandler nextHandler);
    void Handle(ConsoleKey key, Game game);
}

public abstract class InputHandlerBase : IInputHandler
{
    private IInputHandler? _next;

    public void SetNext(IInputHandler nextHandler) => _next = nextHandler;

    public virtual void Handle(ConsoleKey key, Game game)
    {
        _next?.Handle(key, game);
    }
}