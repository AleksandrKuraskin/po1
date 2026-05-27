namespace ConsoleRpg.Shared.Systems.Stats.Modifiers;

public class PercentModifier(float percent) : IStatModifier
{
    private readonly float _percent = percent;
    
    public int OrderId { get; } = 200;
    
    public int Apply(int current, int baseValue) => (int) (current * (1f + _percent));
}