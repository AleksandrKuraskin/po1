using System;
using System.Collections.Generic;
using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.Entities.Enemies;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.Items;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Renderers.Components;
using ConsoleRpg.IO.States;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Logging.Loggers;
using Spectre.Console;

namespace ConsoleRpg.Core;

public class Game(
    MapContext mapContext, 
    Player player, 
    IInputState initialState, 
    IInputHandler globalInputHandler, 
    List<ActionInfo> globalInstructions,
    ConsoleRenderer renderer,
    ConsoleLogger logger,
    string logFilePath
    )
{
    public IRenderer Renderer { get; private set; } = renderer;
    private bool _running = true;
    public MapContext MapContext { get; } = mapContext;
    public Player Player { get; } = player;
    public IInputState CurrentInputState {get; private set;} = initialState;

    public IInputHandler GlobalInputHandler { get; } = globalInputHandler;
    public List<ActionInfo> GlobalInstructions { get; } = globalInstructions;

    public ConsoleLogger Logger { get; } = logger;
    
    public string LogFilePath { get; } = logFilePath;

    public void ChangeRenderer(IRenderer newRenderer)
    {
        Renderer = newRenderer;
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
                Renderer.Render(this);
                var key = Console.ReadKey(true).Key;
                var command = CurrentInputState.HandleInput(key);
                command.Execute(this);
                
                Renderer.Render(this);
            }
        });
    }
    
    public void ProcessEnemiesTurn()
    {
        var enemies = MapContext.Map.GetAllEnemies();
        
        foreach (var enemy in enemies)
        {
            if (enemy.ActedThisTurn)
            {
                LogManager.Instance.Log(
                    $"{enemy.Name} already moved this turn."
                );
                enemy.ActedThisTurn = false;
                continue;
            }

            enemy.TakeTurn(MapContext.Map);
        }
    }

    public void Exit()
    {
        _running = false;
    }
}