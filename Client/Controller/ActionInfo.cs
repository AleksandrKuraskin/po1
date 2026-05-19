using System;
using ConsoleRpg.Client.Controller.Commands;

namespace ConsoleRpg.Client.Controller;

public readonly struct ActionInfo(ConsoleKey key, ICommand command, string description)
{
    public ConsoleKey Key { get; init; } = key;
    public ICommand Command { get; init; } = command;
    public string Description { get; init; } = description;

}