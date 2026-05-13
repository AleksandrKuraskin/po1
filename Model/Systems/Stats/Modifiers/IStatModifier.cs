namespace ConsoleRpg.Model.Systems.Stats.Modifiers;

public interface IStatModifier
{
    int OrderId { get; }
    public int Apply(int current, int baseValue); 
}