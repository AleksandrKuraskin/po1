using System;
using ConsoleRpg.Entities;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.Items;
using ConsoleRpg.Core.Logger;
using Spectre.Console;

namespace ConsoleRpg.Core;

public class Game
{
    private readonly ConsoleRenderer _renderer;
    private readonly IInputHandler _inputHandler;
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
        
        Player = new Player(0, 0);
        Map.SpawnPlayer(Player);
        
        var gameHandler = new GameHandler();
        _inputHandler = gameHandler;
    }
    public void Run()
    {
        AnsiConsole.AlternateScreen(() =>
        {
            while (_running)
            {
                _renderer.Render(this);
                if (!Console.KeyAvailable)
                {
                    continue;
                }
                var key = Console.ReadKey(true).Key;
                _inputHandler.Handle(key, this);
                _renderer.Render(this);
            }
        });
    }

    public void Exit()
    {
        _running = false;
    }
}