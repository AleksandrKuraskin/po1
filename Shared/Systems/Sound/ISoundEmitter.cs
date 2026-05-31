using ConsoleRpg.Shared.Core;
using ConsoleRpg.Shared.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Shared.Systems.Sound;

public interface ISoundEmitter : IGameObject, ISoundSource
{
    void SetMediator(ISoundMediator mediator);
    
    void MakeNoise(ISoundEvent sound);
}