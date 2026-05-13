using ConsoleRpg.Model.Core;
using ConsoleRpg.Model.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Model.Systems.Sound;

public interface ISoundEmitter : IGameObject, ISoundSource
{
    void SetMediator(ISoundMediator mediator);
    
    void MakeNoise(ISoundEvent sound);
}