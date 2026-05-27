using System.Net.Sockets;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Network.Dtos;

namespace ConsoleRpg.Server;

public interface IServerModel : IGameModel
{
    void ProcessEnemiesTurn();
    IEnumerable<Player> GetAllPlayers();
}
