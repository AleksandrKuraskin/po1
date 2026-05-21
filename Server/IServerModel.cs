using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Server;

public interface IServerModel : IGameModel
{
    void ProcessEnemiesTurn();
    void Broadcast(GameStateDto state);
    IEnumerable<Player> GetAllPlayers();
}
