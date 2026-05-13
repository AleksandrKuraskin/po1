using System;
using System.Collections.Generic;
using ConsoleRpg.Model.Core;
using ConsoleRpg.Controller.Commands;
using ConsoleRpg.Controller.Handlers;

namespace ConsoleRpg.Controller.States;

public interface IInputState
{
    public string Name { get; }
    public List<ActionInfo> Instructions { get; }

    string GetInstructions();
    
    IInputState GetNewState(Game game);
    ICommand HandleInput(ConsoleKey key);
}