namespace ConsoleRpg.Systems.Sound;

public interface ISoundListener
{
    void OnHeardSound((int X, int Y) source, int distance, string sourceName);
}