using System.Collections.Generic;
using ConsoleRpg.Shared.Maps;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Logging;
using ConsoleRpg.Shared.Systems.Logging.Loggers;
using ConsoleRpg.Shared.Systems.Network;

namespace ConsoleRpg.Shared.Core;

public interface IGameModel : IStatePublisher
{
    Player Player { get; }
    MapContext MapContext { get; }
    ConsoleLogger Logger { get; }
    string LogFilePath { get; }
    
    void Exit();
}
