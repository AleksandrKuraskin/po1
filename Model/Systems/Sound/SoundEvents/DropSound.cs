using ConsoleRpg.Model.Items;

namespace ConsoleRpg.Model.Systems.Sound.SoundEvents;

public class DropSound(ISoundEmitter emitter, IItem target) : ISoundEvent
{
    public ISoundEmitter Emitter { get; } = emitter;
    
    public string GetFullDescription() => $"{Emitter.Name} dropping {target.Name}";
}