using System.Text;
using ConsoleRpg.Core;

namespace ConsoleRpg.IO;

public class ConsoleRenderer : IRenderer
{
    public void Render(Map map)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                sb.Append(map.GetTile(x, y).GetSymbol());
            }
            sb.AppendLine();
        }
        
        Console.SetCursorPosition(0, 0);
        Console.Write(sb.ToString());
    }
}