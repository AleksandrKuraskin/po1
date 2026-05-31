namespace ConsoleRpg.Shared.Systems.Network;

public class NetworkMessage(string commandName, string payload)
{
    public string CommandName { get; set; } = commandName;
    public string Payload { get; set; } = payload;
}
