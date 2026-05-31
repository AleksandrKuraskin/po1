using ConsoleRpg.Shared.Systems.Network.Dtos;

namespace ConsoleRpg.Shared.Core;

public interface INetworkObserver
{
    void OnStateReceived(GameStateDto state);
}
