using ConsoleRpg.Items;

namespace ConsoleRpg.Systems.Sound.SoundEvents;

public class AttackSound(ISoundEmitter emitter, IItem weapon) : ISoundEvent
{
    public ISoundEmitter Emitter { get; } = emitter;
    
    public string GetFullDescription() => $"{Emitter.Name} attacking with {weapon.Name}";
}