namespace ConsoleRpg.Shared.Maps;

public struct Room(int x, int y, int width, int height)
{
    public int X = x;
    public int Y = y;
    public int Width = width;
    public int Height = height;
    
    public int CenterX => X + Width / 2;
    public int CenterY => Y + Height / 2;
}