using System;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Network;
using System.Text.Json;

namespace ConsoleRpg.Client.Controller.Commands;

public class MoveCommand(int dx, int dy) : IServerCommand
{
    private readonly int _dx = dx;
    private readonly int _dy = dy;

    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("MOVE", JsonSerializer.Serialize(new { dx = _dx, dy = _dy })));
    }
}
