using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Shared.Core;

public interface INetworkObserver
{
    void OnStateReceived(GameState state);
}
