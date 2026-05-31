namespace ConsoleRpg.Shared.Systems.Sound.SoundEvents;

public class AbstractSound(ISoundEmitter emitter) : ISoundEvent
{
    public ISoundEmitter Emitter { get; } = emitter;
    public virtual string GetFullDescription() => $"{Emitter.Name} making a sound";
}