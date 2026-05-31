namespace ConsoleRpg.Shared.Systems.Sound.SoundEvents;

public class MoveSound(ISoundEmitter emitter) : ISoundEvent
{
    public ISoundEmitter Emitter { get; } = emitter;
    
    public string GetFullDescription() => $"{Emitter.Name} moving";
}