using ConsoleRpg.Core;
using ConsoleRpg.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Systems.Sound;

public interface ISoundEmitter : IGameObject, ISoundSource
{
    void SetMediator(ISoundMediator mediator);
    
    void MakeNoise(ISoundEvent sound);
}