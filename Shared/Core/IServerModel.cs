using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Shared.Core;

public interface IServerModel : IGameModel
{
    void ProcessEnemiesTurn();
    void Broadcast(GameState state);
}
