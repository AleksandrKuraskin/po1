using ConsoleRpg.Shared.Map;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging.Loggers;

namespace ConsoleRpg.Shared.Core;

public interface IGameModel : IStatePublisher
{
    Player Player { get; }
    MapContext MapContext { get; }
    ConsoleLogger Logger { get; }
    string LogFilePath { get; }
    
    void Exit();
}
