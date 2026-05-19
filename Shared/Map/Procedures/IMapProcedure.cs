using ConsoleRpg.Shared.Maps;

namespace ConsoleRpg.Shared.Maps.Procedures;

public interface IMapProcedure
{
    void Apply(MapContext context);
}
