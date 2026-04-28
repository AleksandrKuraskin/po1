namespace ConsoleRpg.Entities.Enemies;

public interface ISpeciesObserver
{
    void OnMemberDied(Enemy member);
    void OnMemberMoved((int X, int Y)newCenter);
}