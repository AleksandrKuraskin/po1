// See https://aka.ms/new-console-template for more information

using ConsoleRpg.Core;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Currency;

var game = new GameInitializer().CreateGame();
game.Run();