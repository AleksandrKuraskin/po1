// See https://aka.ms/new-console-template for more information

using ConsoleRpg.Core;
using ConsoleRpg.Items;
using ConsoleRpg.Items.Currency;

var game = new Game();
game.Map.GetTile(0, 1).AddItem(new Gold(50));
game.Map.GetTile(2, 1).AddItem(new Coin(100));
game.Run();