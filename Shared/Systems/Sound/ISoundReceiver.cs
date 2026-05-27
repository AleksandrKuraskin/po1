using ConsoleRpg.Shared.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Shared.Systems.Sound;

public interface ISoundReceiver
{
    int X { get; }
    int Y { get; }
    
    void SetMediator(ISoundMediator mediator);
    void OnHeardSound(ISoundEmitter emitter, (int X, int Y) origin, int distance, ISoundEvent sound);
}