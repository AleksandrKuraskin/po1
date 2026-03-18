using System;
using ConsoleRpg.Core;

namespace ConsoleRpg.IO.Handlers;

public interface IInputHandler
{
    void Handle(ConsoleKey key, Game game);
}