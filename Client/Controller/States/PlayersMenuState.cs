using System;
using System.Collections.Generic;
using ConsoleRpg.Client.Controller;
using ConsoleRpg.Shared.Core;
using ConsoleRpg.Client.Controller.Commands;
using ConsoleRpg.Client.Controller.Handlers;
using ConsoleRpg.Client.View;

namespace ConsoleRpg.Client.Controller.States;

public class PlayersMenuState : IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputState _previousState;
    
    public string Name => "Players Menu";
    public List<ActionInfo> Instructions { get; }

    public PlayersMenuState(IInputState previousState, IRenderer previousRenderer, IInputHandler globalChain, List<ActionInfo> globalInstructions)
    {
        _previousState = previousState;
        Instructions = new List<ActionInfo>(globalInstructions);
        
        var back = new KeyBindHandler(
            new ActionInfo(ConsoleKey.O, new CloseJournalCommand(previousState, previousRenderer), "Close Players Menu"), 
            Instructions
        );
        
        back.SetNext(globalChain);
        _inputChain = back;
    }

    public string GetInstructions()
    {
        var output = "";
        foreach (var instruction in Instructions)
        {
            var instructionString = instruction.Key + " - " + instruction.Description;
            output = output + "\n" + instructionString;
        }
        return output;
    }
    
    public IInputState GetNewState(IGameModel game) => _previousState; 
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}
