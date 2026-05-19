using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Logging;

namespace ConsoleRpg.Client.Controller.Commands;

public class LogMessageCommand(string message, LogType type = LogType.Info) : ILocalCommand
{
    private readonly string _message = message;
    private readonly LogType _type = type;

    public void ExecuteLocal(IClientModel model)
    {
        LogManager.Instance.Log(_message, _type);
    }
}
