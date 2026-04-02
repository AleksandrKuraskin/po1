using System;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.Items;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.IO.States;
using Spectre.Console;

namespace ConsoleRpg.Core;

public class Game
{
    private readonly ConsoleRenderer _renderer;
    private bool _running;
    public MapContext MapContext { get; }
    public Logger.Logger Logger { get; }
    public Player Player { get; }
    public IInputState CurrentInputState {get; private set;}

    public IInputHandler GlobalInputHandler { get; }
    public List<ActionInfo> GlobalInstructions { get; } = new();

    private IInputHandler InitializeInputHandler()
    {
        var escapeGame = new KeyBindHandler(new ActionInfo(ConsoleKey.Escape, new ExitGameCommand(), "Exit game"), GlobalInstructions);
        return escapeGame;
    }
    public Game()
    {
        ItemFactory.Initialize();
        var builder = new MapBuilder();
        var director = new MapDirector(builder);
        GlobalInputHandler = InitializeInputHandler();
        _renderer = new ConsoleRenderer();
        director.ConstructRandomMap();
        MapContext = builder.Build();
        CurrentInputState = new MoveState(MapContext, GlobalInputHandler, GlobalInstructions);
        _running = true;
        Logger = new Logger.Logger();

        var spawn = MapContext.SpawnPoint;
        Player = new Player(spawn.x, spawn.y);
        MapContext.Map.SpawnPlayer(Player);
    }

    public void ChangeInputState(IInputState newState)
    {
        CurrentInputState = newState;
    }
    
    public void Run()
    {
        Console.CursorVisible = false;
        AnsiConsole.AlternateScreen(() =>
        {
            while (_running)
            {
                _renderer.Render(this);
                var key = Console.ReadKey(true).Key;
                var command = CurrentInputState.HandleInput(key);
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