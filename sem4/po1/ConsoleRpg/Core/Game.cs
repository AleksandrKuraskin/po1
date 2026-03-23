using System;
using ConsoleRpg.Entities;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.Items;
using ConsoleRpg.Core.Logger;
using ConsoleRpg.IO.States;
using Spectre.Console;

namespace ConsoleRpg.Core;

public class Game
{
    private readonly ConsoleRenderer _renderer;
    private IInputState _currentInputState;
    private bool _running;
    public Map Map { get; set; }
    public Logger.Logger Logger { get; set; }
    public Player Player { get; }
    

    public Game()
    {
        Console.CursorVisible = false;
        ItemFactory.Initialize();
        Map = new Map();
        Logger = new Logger.Logger();
        _renderer = new ConsoleRenderer();
        _running = true;
        _currentInputState = new MoveState();
        
        Player = new Player(0, 0);
        Map.SpawnPlayer(Player);
    }

    public void ChangeInputState(IInputState newState)
    {
        _currentInputState = newState;
    }
    
    public void Run()
    {
        AnsiConsole.AlternateScreen(() =>
        {
            while (_running)
            {
                _renderer.Render(this);
                var key = Console.ReadKey(true).Key;
                var command = _currentInputState.HandleInput(key);
                command.Execute(this);
                _renderer.Render(this);
            }
        });
    }

    public void Exit()
    {
        _running = false;
    }
}