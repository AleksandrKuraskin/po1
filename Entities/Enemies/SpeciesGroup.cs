using System.Collections.Generic;
using System.Linq;
using ConsoleRpg.Entities.Enemies.Behaviors;
using ConsoleRpg.Systems.Sound;

namespace ConsoleRpg.Entities.Enemies;

public class SpeciesGroup(int maxRadius, IEnemyBehavior behavior) : ISpeciesPublisher
{
    private readonly List<ISpeciesObserver> _members = new();
    public readonly int MaxRadius = maxRadius;
    
    public IEnemyBehavior Behavior { get; set; } = behavior;

    public void Attach(ISpeciesObserver member)
    {
        _members.Add(member);
        NotifyMemberMove(member);
    }
    public void Detach(ISpeciesObserver member)
    {
        _members.Remove(member);
        NotifyMemberDeath(member);
    }
    
    public void NotifyMemberDeath(ISpeciesObserver member)
    {
        foreach (var livingMembers in _members)
        {
            livingMembers.OnMemberDied(member);
        }
    }

    public void NotifyMemberMove(ISpeciesObserver member)
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