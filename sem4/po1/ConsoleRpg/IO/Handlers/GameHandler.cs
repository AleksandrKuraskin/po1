using System;
using ConsoleRpg.Core;
using ConsoleRpg.IO.States;

namespace ConsoleRpg.IO.Handlers;

public class GameHandler : IInputHandler
{
    private IInputState _currentState;

    public GameHandler()
    {
        _currentState = new MoveState();
    }

    public void SetState(IInputState state)
    {
        _currentState = state;
    }

    public void Handle(ConsoleKey key, Game game)
    {
        if (key == ConsoleKey.Escape)
        {
            game.Exit();
            return;
        }
        
        var command = _currentState.HandleInput(key, this);
        command.Execute(game);
    }
}