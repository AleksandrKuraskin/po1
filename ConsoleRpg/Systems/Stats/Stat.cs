using ConsoleRpg.Systems.Stats;
using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Systems.Stats;

public class Stat(int value)
{
    public int BaseValue { get; set; } = value;
    public int Value {
        get
        {
            var finalValue = BaseValue;
            foreach (var modifier in _modifiers)
            {
                finalValue = modifier.Apply(finalValue, BaseValue);
            }
            return Math.Max(0, finalValue);
        }
    }

    private List<IStatModifier> _modifiers = new();

    public void Decrease(int amount)
    {
        BaseValue -= amount;
        if (BaseValue < 0) 
            BaseValue = 0;
    }
    
    public void Increase(int amount)
    {
        BaseValue += amount;
    }
    
    public void SetBaseValue(int newValue)
    {
        BaseValue = Math.Max(0, newValue);
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