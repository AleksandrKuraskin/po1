using ConsoleRpg.Shared.Core;

namespace ConsoleRpg.Shared.Systems.Sound;

public interface ISoundSource
{
    Loudness Loudness { get; }
}