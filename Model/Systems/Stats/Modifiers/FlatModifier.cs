namespace ConsoleRpg.Model.Systems.Stats.Modifiers;

public class FlatModifier(int value) : IStatModifier
{
    private readonly int _value = value;
    
    public int OrderId { get; } = 100;
    public int Apply(int current, int baseValue) =>  current + _value;
}