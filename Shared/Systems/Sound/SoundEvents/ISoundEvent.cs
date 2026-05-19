namespace ConsoleRpg.Shared.Systems.Sound.SoundEvents;

public interface ISoundEvent
{
    ISoundEmitter Emitter { get; }
    string GetFullDescription();
}