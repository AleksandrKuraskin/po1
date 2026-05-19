using System;
using ConsoleRpg.Server;
using ConsoleRpg.Client;
using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Maps.Themes;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Client.Controller;
using ConsoleRpg.Client.Controller.Handlers;
using ConsoleRpg.Client.Controller.Commands;
using System.Collections.Generic;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using System.IO;
using System.Threading;
using ConsoleRpg.Client.View;
using ConsoleRpg.Server.CommandHandlers;
using Spectre.Console;

Console.WriteLine("Run as (S)erver or (K)lient?");
var choice = Console.ReadKey(true).Key;

if (choice == ConsoleKey.S)
{
    var config = new GameConfig();
    config.Read();
    var logger = new ConsoleLogger();
    
    if (!Directory.Exists("Server")) Directory.CreateDirectory("Server");
    var logFilePath = Path.Combine("Server", "server.log");
    LogManager.Instance.Attach(new FileLogger("Server", "Server", "server.log"));
    LogManager.Instance.Attach(logger);
    
    var builder = new MapBuilder();
    var director = new MapDirector(builder);
    var theme = ThemeProvider.GetRandomTheme();
    theme.ApplyGenerationStrategy(director);
    var mapContext = builder.Build();

    var dispatcher = new CommandDispatcher();
    dispatcher.RegisterHandler(new MoveCommandHandler());
    dispatcher.RegisterHandler(new JoinCommandHandler());
    dispatcher.RegisterHandler(new EquipCommandHandler());
    dispatcher.RegisterHandler(new DropCommandHandler());
    dispatcher.RegisterHandler(new SwapCommandHandler());
    dispatcher.RegisterHandler(new PickUpCommandHandler());
    dispatcher.RegisterHandler(new AttackCommandHandler());
    dispatcher.RegisterHandler(new ExitCommandHandler());
    
    var server = new ServerModel(dispatcher, mapContext, logger, logFilePath, 5555);
    _ = server.Start();
    
    Console.WriteLine("Server is running. Press any key to exit.");
    Console.ReadKey();
}
else if (choice == ConsoleKey.K)
{
    Console.Write("Enter Server IP (default 127.0.0.1): ");
    var ip = Console.ReadLine();
    if (string.IsNullOrEmpty(ip)) ip = "127.0.0.1";
    
    Console.Write("Enter Player Name: ");
    var name = Console.ReadLine();
    if (string.IsNullOrEmpty(name)) name = "Player";

    var config = new GameConfig();
    config.Read();
    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    var logFileName = $"{name}_{timestamp}.txt";
    LogManager.Instance.Attach(new FileLogger(name, config.LogDirectory, logFileName));

    var globalInstructions = new List<ActionInfo>();
    var globalInputHandler = new KeyBindHandler(
        new ActionInfo(ConsoleKey.Escape, new ExitGameCommand(), "Exit game"), 
        globalInstructions);

    var client = new ClientModel(ip, 5555, name, globalInputHandler, globalInstructions);
    var viewObserver = new ViewObserver(client);
    client.Attach(viewObserver);
    
    AnsiConsole.Cursor.Hide();
    client.Renderer.Render(client);
    
    while (true)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            var command = client.CurrentInputState.HandleInput(key);
            command.Execute(client);
            client.Notify();
        }
        else
        {
            Thread.Sleep(16);
        }
    }
}
