namespace ConsoleRpg.Entities.Enemies;

public interface ISpeciesPublisher
{
    void Attach(Enemy member);
    void Detach(Enemy member);
    void NotifyMemberDeath(Enemy member);
    void NotifyMemberMove(Enemy member);
}