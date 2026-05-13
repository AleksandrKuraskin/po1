// See https://aka.ms/new-console-template for more information

using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Items;
using ConsoleRpg.Model.Items.Currency;

var game = new GameInitializer().CreateGame();
game.Run();