using ConsoleRpg.Items;

namespace ConsoleRpg.Systems.Sound.SoundEvents;

public class AttackSound(ISoundEmitter emitter, IItem weapon) : AbstractSound(emitter)
{
    public string GetFullDescription() => $"{Emitter.Name} attacking with {weapon.Name}";
}