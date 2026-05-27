namespace ConsoleRpg.Shared.Core;

public interface IStatePublisher
{
    void Attach(IStateObserver observer);
    void Detach(IStateObserver observer);
    void Notify();
}
