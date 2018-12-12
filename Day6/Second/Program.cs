using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Second
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = DateTime.Now;
            string[] inputLines;
            using (var sr = new StreamReader("input.txt"))
            {
                var input = sr.ReadToEnd();
                inputLines = input.Split("\n");
            }

            var masterPoints = GetInputPoints(inputLines);
            var mapPoints = GenerateMap(masterPoints);
            var pointsWithCalculatedDistance = CalculateDistanceToEachCoordinate(mapPoints, masterPoints);
            var safestPoint = pointsWithCalculatedDistance.Where(p => p.Distance < 10000);
            var safeAreaSize = safestPoint.Count();
            Console.WriteLine(safeAreaSize.ToString());
            Console.WriteLine((DateTime.Now - start).ToString(@"mm\:ss"));
        }

        public static Point[] GetInputPoints(string[] input)
        {
            var points = new List<Point>();
            for (int i = 0; i < input.Length; i++)
            {
                if (!string.IsNullOrEmpty(input[i]))
                {
                    string[] coordinates = input[i].Trim().Split(',');
                    points.Add(new Point() { X = Int32.Parse(coordinates[0]), Y = Int32.Parse(coordinates[1]), Id = i + 1, IsMaster = true });
                }
            }

            return points.ToArray();
        }

        public static Point[] GenerateMap(Point[] masterPoints)
        {
            var (minX, maxX, minY, maxY) = GetMapBounds(masterPoints);
            var points = new List<Point>();

            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    if (masterPoints.Where(p => p.X == j && p.Y == i).Count() == 0)
                    {
                        points.Add(new Point() { X = j, Y = i });
                    }
                }
            }

            points.AddRange(masterPoints);
            return points.ToArray();
        }

        public static Point[] CalculateDistanceToEachCoordinate(Point[] allPoints, Point[] masterPoints)
        {
            foreach (var point in allPoints)
            {
                foreach (var coordinate in masterPoints)
                {
                    point.Distance += CalculateDistanceToCoordinate(point, coordinate);
                }
            }

            return allPoints;
        }

        public static int CalculateDistanceToCoordinate(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        public static (int, int, int, int) GetMapBounds(Point[] masterPoints)
        {
            int minX = masterPoints.Min(p => p.X);
            int maxX = masterPoints.Max(p => p.X);
            int minY = masterPoints.Min(p => p.Y);
            int maxY = masterPoints.Max(p => p.Y);
            return (minX, maxX, minY, maxY);
        }

        public class Point
        {
            public int X { get; set; }

            public int Y { get; set; }

            public int Id { get; set; } = 0;

            public int Distance { get; set; } = 0;

            public bool IsMaster { get; set; } = false;
        }
    }
}
