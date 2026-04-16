using ConsoleRpg.Core.Map;
using ConsoleRpg.Entities;
using ConsoleRpg.IO.Commands;
using ConsoleRpg.IO.Handlers;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO.States;
using ConsoleRpg.Items;
using ConsoleRpg.Systems.Logging;
using ConsoleRpg.Systems.Logging.Loggers;

namespace ConsoleRpg.Core;

public class GameInitializer
{
    private readonly GameConfig _config = new ();
    
    public Game CreateGame()
    {
        ItemFactory.Initialize();

        var consoleLogger = new ConsoleLogger();

        LogManager.Instance.Attach(consoleLogger);
        LogManager.Instance.Attach(new FileLogger(_config.PlayerName, _config.LogDirectory));
        
        var globalInstructions = new List<ActionInfo>();
        var globalInputHandler = new KeyBindHandler(
            new ActionInfo(ConsoleKey.Escape, new ExitGameCommand(), "Exit game"), 
            globalInstructions);
        
        var builder = new MapBuilder();
        var director = new MapDirector(builder);
        director.ConstructRandomMap();
        var mapContext = builder.Build();

        var spawn = mapContext.SpawnPoint;
        var player = new Player(spawn.x, spawn.y);
        mapContext.Map.SpawnPlayer(player);

        var initialState = new MoveState(mapContext, globalInputHandler, globalInstructions);

        var renderer = new ConsoleRenderer();
        
        return new Game(
            mapContext, 
            player, 
            initialState, 
            globalInputHandler, 
            globalInstructions, 
            renderer,
            consoleLogger
            );

    }
    
}