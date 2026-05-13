using ConsoleRpg.Model.Items;

namespace ConsoleRpg.Model.Systems.Sound.SoundEvents;

public class AttackSound(ISoundEmitter emitter, IItem weapon) : AbstractSound(emitter)
{
    public string GetFullDescription() => $"{Emitter.Name} attacking with {weapon.Name}";
}