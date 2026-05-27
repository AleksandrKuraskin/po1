using System.Text.Json;
using ConsoleRpg.Shared.Entities;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Server.CommandHandlers;

public class MoveCommandHandler : IServerCommandHandler
{
    public string CommandName => "MOVE";

    public void Handle(string payload, IServerModel server, Player player)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<MoveData>(payload, options);
        if (data != null)
        {
            if (server.MapContext.Map.TryMovePlayer(player, data.Dx, data.Dy))
            {
                player.MakeNoise(new MoveSound(player));
            }
            server.ProcessEnemiesTurn();
        }
    }

    private class MoveData { public int Dx { get; set; } public int Dy { get; set; } }
}
