using ConsoleRpg.Items;

namespace ConsoleRpg.Systems.Sound.SoundEvents;

public class PickUpSound(ISoundEmitter emitter, IItem target) : ISoundEvent
{
    public ISoundEmitter Emitter { get; } = emitter;
    
    public string GetFullDescription() => $"{Emitter.Name} picking up {target.Name}";
}