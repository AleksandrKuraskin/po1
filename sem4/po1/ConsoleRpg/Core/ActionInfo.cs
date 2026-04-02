using ConsoleRpg.IO.Commands;

namespace ConsoleRpg.Core;

public struct ActionInfo(ConsoleKey key, ICommand command, string description)
{
    public ConsoleKey Key { get; init; } = key;
    public ICommand Command { get; init; } = command;
    public string Description { get; init; } = description;

}