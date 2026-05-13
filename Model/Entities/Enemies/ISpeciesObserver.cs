using ConsoleRpg.Model.Core;

namespace ConsoleRpg.Model.Entities.Enemies;

public interface ISpeciesObserver : IEntity
{
    void OnMemberDied(ISpeciesObserver member);
    void OnMemberMoved((int X, int Y)newCenter);
}