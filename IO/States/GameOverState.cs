using System;
using System.Collections.Generic;
using ConsoleRpg.Core;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;

namespace ConsoleRpg.IO.States;

public class GameOverState : IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly string _logPath;
    public string Name => "GAME OVER";
    public List<ActionInfo> Instructions { get; }

    public GameOverState(string logPath)
    {
        _logPath = logPath;
        Instructions = new List<ActionInfo> 
        {
            new ActionInfo(ConsoleKey.Escape, new ExitGameCommand(), "Exit game") 
        };
        
        var exitHandler = new KeyBindHandler(Instructions[0], Instructions);
        exitHandler.SetNext(new NullBind());
        
        _inputChain = exitHandler;
    }

    public IInputState GetNewState(Game game) => this;
    public ICommand HandleInput(ConsoleKey key) => _inputChain.Handle(key);
    public string GetInstructions() => $"YOU DIED! Press Esc to exit.\nYou can see game logs at {_logPath}";
}