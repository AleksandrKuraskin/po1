namespace ConsoleRpg.Model.Entities.Enemies;

public interface ISpeciesPublisher
{
    void Attach(ISpeciesObserver member);
    void Detach(ISpeciesObserver member);
    void NotifyMemberDeath(ISpeciesObserver member);
    void NotifyMemberMove(ISpeciesObserver member);
}