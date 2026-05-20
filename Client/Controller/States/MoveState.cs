using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Map;
using ConsoleRpg.Client.Controller.Commands;
using ConsoleRpg.Client.Controller.Handlers;
using ConsoleRpg.Shared.Systems.Attacking;

namespace ConsoleRpg.Client.Controller.States;

public class MoveState: IInputState
{
    private readonly IInputHandler _inputChain;
    private readonly IInputHandler _globalInputChain;
    private readonly List<ActionInfo> _globalInstructions;
    private IInputHandler _lastHandler;
    
    public string Name { get; } = "Move State";

    public List<ActionInfo> Instructions { get; }

    public MoveState(MapContext context, IInputHandler globalChain, List<ActionInfo> globalInstructions)
    {
        _globalInputChain = globalChain;
        _globalInstructions = globalInstructions;

        Instructions = new List<ActionInfo>(_globalInstructions);
        
        var moveUp = new KeyBindHandler(new ActionInfo(ConsoleKey.W, new MoveCommand(0, -1), "Move up"), Instructions);
        var moveDown = new KeyBindHandler(new ActionInfo(ConsoleKey.S, new MoveCommand(0, 1), "Move down"), Instructions);
        var moveLeft = new KeyBindHandler(new ActionInfo(ConsoleKey.A, new MoveCommand(-1, 0), "Move left"), Instructions);
        var moveRight = new KeyBindHandler(new ActionInfo(ConsoleKey.D, new MoveCommand(1, 0), "Move right"), Instructions);
        var openJournal = new KeyBindHandler(
            new ActionInfo(
                ConsoleKey.J,
                new OpenJournalCommand(this),
                "Open Journal"),
            Instructions);

        var openPlayersMenu = new KeyBindHandler(
            new ActionInfo(
                ConsoleKey.O,
                new OpenPlayersMenuCommand(this),
                "Players Menu"),
            Instructions);
        
        moveUp
            .SetNext(moveDown)
            .SetNext(moveLeft)
            .SetNext(moveRight)
            .SetNext(openJournal)
            .SetNext(openPlayersMenu);

        _lastHandler = openPlayersMenu;
        
        if (context.Itemized)
        {
            var swapHands = new KeyBindHandler(new ActionInfo(ConsoleKey.F, new SwapHandsCommand(), "Swap hands"), Instructions);
            var pickUp = new KeyBindHandler(new ActionInfo(ConsoleKey.E, new PickUpCommand(), "Pick up"), Instructions);
            var changeState = new KeyBindHandler(new ActionInfo(ConsoleKey.I, new ChangeStateCommand(this), "Inventory management"), Instructions);
            _lastHandler
                .SetNext(swapHands)
                .SetNext(pickUp)
                .SetNext(changeState);
            _lastHandler = changeState;
        }

        if (context.Dangerous)
        {
            var normalAttack = new KeyBindHandler(new ActionInfo(ConsoleKey.N, new AttackCommand(new NormalAttackVisitor()), "Normal Attack"), Instructions);
            var stealthAttack = new KeyBindHandler(new ActionInfo(ConsoleKey.OemComma, new AttackCommand(new StealthAttackVisitor()), "Stealth Attack"), Instructions);
            var magicAttack = new KeyBindHandler(new ActionInfo(ConsoleKey.M, new AttackCommand(new MagicAttackVisitor()), "Magic Attack"), Instructions);
            
            _lastHandler
                .SetNext(normalAttack)
                .SetNext(stealthAttack)
                .SetNext(magicAttack);
            _lastHandler = magicAttack;
        }

        _lastHandler.SetNext(_globalInputChain);
        
        _inputChain = moveUp;
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

    public IInputState GetNewState(IGameModel game) => new InventoryState(_globalInputChain, _globalInstructions);
    
    public ICommand HandleInput(ConsoleKey key)
    {
        return _inputChain.Handle(key);
    }
}