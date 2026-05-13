using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Model.Systems.Sound;

public interface ISoundSource
{
    Loudness Loudness { get; }
}