using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Systems.Stats;

public class Stat(int value)
{
    private int BaseValue { get; set; } = value;

    private int _value;
    public int Value {
        get
        {
            var finalValue = BaseValue;
            foreach (var modifier in _modifiers)
            {
                finalValue = modifier.Apply(finalValue, BaseValue);
            }
            return finalValue;
        }
        set
        {
            if (value < 0)
                _value = 0;
        }
    }

    private List<IStatModifier> _modifiers = new();

    public void Decrease(int amount)
    {
        Value -= amount;
    }
    
    public void AddModifier(IStatModifier modifier)
    {
        _modifiers.Add(modifier);
        _modifiers.Sort((a, b) => a.OrderId.CompareTo(b.OrderId));
    }

    public void RemoveModifier(IStatModifier modifier)
    {
        _modifiers.Remove(modifier);
    }

}