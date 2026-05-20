using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Server;

public interface IServerModel : IGameModel
{
    void ProcessEnemiesTurn();
    void Broadcast(GameState state);
    IEnumerable<Player> GetAllPlayers();
}
