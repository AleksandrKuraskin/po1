using ConsoleRpg.Shared.Systems.Sound.SoundEvents;

namespace ConsoleRpg.Shared.Systems.Sound;

public interface ISoundMediator
{
    void AddReceiver(ISoundReceiver receiver);
    void RemoveReceiver(ISoundReceiver receiver);
    void EmitSound(ISoundEmitter emitter, (int X, int Y) origin, ISoundEvent sound);
}