using ConsoleRpg.Entities;
using ConsoleRpg.IO.Renderers;
using ConsoleRpg.IO;
using Spectre.Console;

namespace ConsoleRpg.Core;

public class Game
{
    private readonly ConsoleRenderer _renderer;
    private readonly IInputHandler _inputHandler;
    private bool _running;
    public Map Map { get; set; }
    public Logger Logger { get; set; }
    public bool DropModeActive { get; set; } = false;
    public Player Player { get; }
    

    public Game()
    {
        Console.CursorVisible = false;
        Map = new Map();
        Logger = new Logger();
        _renderer = new ConsoleRenderer();
        _running = true;
        
        Player = new Player(0, 0);
        Map.SpawnPlayer(Player);
        
        var gameHandler = new GameHandler();
        var movementHandler = new MovementHandler();
        var actionHandler = new ActionHandler();
        var inventoryHandler = new InventoryHandler();
        
        gameHandler.SetNext(movementHandler);
        movementHandler.SetNext(actionHandler);
        actionHandler.SetNext(inventoryHandler);
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