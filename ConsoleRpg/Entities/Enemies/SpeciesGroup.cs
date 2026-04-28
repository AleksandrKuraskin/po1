namespace ConsoleRpg.Entities.Enemies;

public class SpeciesGroup(int maxRadius) : ISpeciesPublisher
{
    private readonly List<Enemy> _members = new();
    public readonly int MaxRadius = maxRadius;

    public void Attach(Enemy member)
    {
        _members.Add(member);
        NotifyMemberMove(member);
    }
    public void Detach(Enemy member)
    {
        _members.Remove(member);
        NotifyMemberDeath(member);
    }
    
    public void NotifyMemberDeath(Enemy member)
    {
        foreach (var livingMembers in _members)
        {
            livingMembers.OnMemberDied(member);
        }
    }

    public void NotifyMemberMove(Enemy member)
    {
        foreach (var m in _members)
        {
            m.OnMemberMoved(GetGroupCenter());
        }
    }

    public (int X, int Y) GetGroupCenter()
    {
        if (_members.Count == 0) return (0, 0);

        var avgX = (int)_members.Average(member => member.X);
        var avgY = (int)_members.Average(member => member.Y);

        return (avgX, avgY);
    }
}