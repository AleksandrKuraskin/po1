using System;
using System.Collections.Generic;

using ConsoleRpg.Shared.Maps;
namespace ConsoleRpg.Shared.Maps.Procedures;

public class RoomsProcedure : IMapProcedure
{
    private const int _minRoomSize = 4;
    private const int _minSegmentSize = 8;
    private const int _maxSegmentSize = 14;
    private readonly Random _rng = new Random();

    private class Segment(int x, int y, int w, int h)
    {
        public readonly int X = x;
        public readonly int Y = y;
        public readonly int Width = w;
        public readonly int Height = h;
        public Segment? Left, Right;
    }

    public void Apply(MapContext context)
    {
        var root = new Segment(1, 1, context.Map.Width - 2, context.Map.Height - 2);
        var segments = new List<Segment> { root };
        var split = true;
        
        while (split)
        {
            split = false;
            var currentSegments = new List<Segment>(segments);

            foreach (var segment in currentSegments)
            {
                if (segment.Left == null && segment.Right == null && (segment.Width > _maxSegmentSize || segment.Height > _maxSegmentSize))
                {
                    if (Split(segment))
                    {
                        segments.Add(segment.Left!);
                        segments.Add(segment.Right!);
                        split = true;
                    }
                }
            }
        }

        foreach (var segment in segments)
        {
            if (segment.Left == null && segment.Right == null)
            {
                var maxW = segment.Width - 2;
                var maxH = segment.Height - 2;
                
                var minW = Math.Min(_minRoomSize, maxW);
                var minH = Math.Min(_minRoomSize, maxH);

                var roomW = _rng.Next(minW, maxW + 1);
                var roomH = _rng.Next(minH, maxH + 1);
                
                var roomX = segment.X + 1 + _rng.Next(0, (segment.Width - 1) - roomW);
                var roomY = segment.Y + 1 + _rng.Next(0, (segment.Height - 1) - roomH);

                var room = new Room(roomX, roomY, roomW, roomH);
                context.Rooms.Add(room);

                for (var y = room.Y; y < room.Y + room.Height; y++)
                {
                    for (var x = room.X; x < room.X + room.Width; x++)
                    {
                        context.Map.GetTile(x, y).IsWall = false;
                    }
                }
            }
        }
        
        if (context.Rooms.Count > 0)
        {
            var randomRoom = context.Rooms[_rng.Next(context.Rooms.Count)];
            context.SpawnPoint = (randomRoom.CenterX, randomRoom.CenterY);
        }
    }

    private bool Split(Segment segment)
    {
        var splitH = _rng.NextDouble() > 0.5;
        if (segment.Width > segment.Height && (double)segment.Width / segment.Height >= 1.25) splitH = false;
        else if (segment.Height > segment.Width && (double)segment.Height / segment.Width >= 1.25) splitH = true;

        var max = (splitH ? segment.Height : segment.Width) - _minSegmentSize;
        if (max <= _minSegmentSize) return false;

        var splitPos = _rng.Next(_minSegmentSize, max);

        if (splitH)
        {
            segment.Left = new Segment(segment.X, segment.Y, segment.Width, splitPos);
            segment.Right = new Segment(segment.X, segment.Y + splitPos, segment.Width, segment.Height - splitPos);
        }
        else
        {
            segment.Left = new Segment(segment.X, segment.Y, splitPos, segment.Height);
            segment.Right = new Segment(segment.X + splitPos, segment.Y, segment.Width - splitPos, segment.Height);
        }
        return true;
    }
}