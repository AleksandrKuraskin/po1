using ConsoleRpg.Core;

namespace ConsoleRpg.Entities.Enemies;

public interface ISpeciesObserver : IEntity
{
    void OnMemberDied(ISpeciesObserver member);
    void OnMemberMoved((int X, int Y)newCenter);
}