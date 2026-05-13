namespace ConsoleRpg.Model.Systems.Sound.SoundEvents;

public interface ISoundEvent
{
    ISoundEmitter Emitter { get; }
    string GetFullDescription();
}