using ConsoleRpg.Shared.Systems.Network;
using System.Text.Json;
using ConsoleRpg.Shared.Systems.Attacking;

namespace ConsoleRpg.Client.Controller.Commands;

public class AttackCommand(IAttackVisitor attackVisitor) : IServerCommand
{
    private readonly IAttackVisitor _attackVisitor = attackVisitor;

    public void ExecuteServer(IClientModel model)
    {
        model.SendMessage(new NetworkMessage("ATTACK", JsonSerializer.Serialize(new { VisitorName = _attackVisitor.Name })));
    }
}
