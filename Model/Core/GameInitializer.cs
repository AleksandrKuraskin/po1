using System;
using System.Collections.Generic;
using System.IO;
using ConsoleRpg.Model.Core.Map;
using ConsoleRpg.Model.Core.Map.Themes;
using ConsoleRpg.Model.Entities;
using ConsoleRpg.Controller.Commands;
using ConsoleRpg.Controller.Handlers;
using ConsoleRpg.View.Renderers;
using ConsoleRpg.Controller.States;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Systems.Logging;
using ConsoleRpg.Model.Systems.Logging.Loggers;

namespace ConsoleRpg.Model.Core;

public class GameInitializer
{
    private readonly GameConfig _config = new();

    public GameInitializer()
    {
        _config.Read();
    }
    
    public Game CreateGame()
    {
        var consoleLogger = new ConsoleLogger();
        
        var logFileName = $"{_config.PlayerName}_{DateTime.Now:yyyyMMdd_HHmmss}.log";
        var logFilePath = Path.Combine(_config.LogDirectory, logFileName);

        LogManager.Instance.Attach(consoleLogger);
        LogManager.Instance.Attach(new FileLogger(_config.PlayerName, _config.LogDirectory, logFileName));
        
        var globalInstructions = new List<ActionInfo>();
        var globalInputHandler = new KeyBindHandler(
            new ActionInfo(ConsoleKey.Escape, new ExitGameCommand(), "Exit game"), 
            globalInstructions);
        
        var theme = ThemeProvider.GetRandomTheme();
        var builder = new MapBuilder();
        var director = new MapDirector(builder);
        
        theme.ApplyGenerationStrategy(director);
        
        var mapContext = builder.Build();

        var spawn = mapContext.SpawnPoint;
        var player = new Player(spawn.x, spawn.y, _config.PlayerName);
        if(mapContext.SoundMediator != null)
            player.SetMediator(mapContext.SoundMediator);
        
        mapContext.Map.SpawnPlayer(player);
        
        LogManager.Instance.Log(theme.IntroMessage);

        var initialState = new MoveState(mapContext, globalInputHandler, globalInstructions);
        var renderer = new ConsoleRenderer();
        
        return new Game(
            mapContext, 
            player, 
            initialState, 
            globalInputHandler, 
            globalInstructions, 
            renderer,
            consoleLogger,
            logFilePath
            );
    }
    
}