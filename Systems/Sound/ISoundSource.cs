using ConsoleRpg.Core;

namespace ConsoleRpg.Systems.Sound;

public interface ISoundSource
{
    Loudness Loudness { get; }
}