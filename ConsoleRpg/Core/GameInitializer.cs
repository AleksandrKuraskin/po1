using ConsoleRpg.Core.Map;
using ConsoleRpg.Core.Map.Themes;
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
    private readonly GameConfig _config = new();

    public GameInitializer()
    {
        _config.Read();
    }
    
    public Game CreateGame()
    {
        var consoleLogger = new ConsoleLogger();

        LogManager.Instance.Attach(consoleLogger);
        LogManager.Instance.Attach(new FileLogger(_config.PlayerName, _config.LogDirectory));
        
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
        var player = new Player(spawn.x, spawn.y);
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
            consoleLogger
            );

    }
    
}