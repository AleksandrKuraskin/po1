using System.Collections.Generic;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.View;
using ConsoleRpg.Client.Controller.States;
using ConsoleRpg.Client.Controller.Handlers;
using ConsoleRpg.Shared.Systems.Network;
using ConsoleRpg.Client.Controller;

namespace ConsoleRpg.Client;

public interface IClientModel : IGameModel
{
    IRenderer Renderer { get; }
    IInputState CurrentInputState { get; }
    IInputHandler GlobalInputHandler { get; }
    List<ActionInfo> GlobalInstructions { get; }
    GameState? LastState { get; }

    void ChangeRenderer(IRenderer newRenderer);
    void ChangeInputState(IInputState newState);
    void SendMessage(NetworkMessage message);
}
