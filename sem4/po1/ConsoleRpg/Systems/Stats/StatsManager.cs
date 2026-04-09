using ConsoleRpg.Systems.Stats.Modifiers;

namespace ConsoleRpg.Systems.Stats;

public class StatsManager
{
    private readonly Dictionary<StatType, Stat> _stats = new();

    public StatsManager AddStat(StatType type, int value)
    {
        if (!_stats.ContainsKey(type))
        {
            _stats[type] = new Stat(value);   
        }

        return this;
    }

    public Stat GetStat(StatType type)
    {
        return _stats.TryGetValue(type, out var stat) ? stat : new Stat(0);
    }
    
    public bool HasStat(StatType type) => _stats.ContainsKey(type);

    public StatsManager AddModifier(StatType type, IStatModifier modifier)
    {
        AddStat(type, 0);
        _stats[type].AddModifier(modifier);
        
        return this;
    }

    public StatsManager RemoveModifier(StatType type, IStatModifier modifier)
    {
        GetStat(type).RemoveModifier(modifier);

        return this;
    }
    
}