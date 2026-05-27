using ConsoleRpg.Shared.Items;

namespace ConsoleRpg.Shared.Systems.Sound.SoundEvents;

public class AttackSound(ISoundEmitter emitter, IItem weapon) : AbstractSound(emitter)
{
    public override string GetFullDescription() => $"{Emitter.Name} attacking with {weapon.Name}";
}