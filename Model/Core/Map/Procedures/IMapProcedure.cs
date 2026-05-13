namespace ConsoleRpg.Model.Core.Map.Procedures;

public interface IMapProcedure
{
    void Apply(MapContext context);
}