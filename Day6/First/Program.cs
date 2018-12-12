using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace First
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
            var processedPoints = ProcessMap(mapPoints);
            var filteredPoints = FilterPoints(processedPoints, masterPoints);
            var pointGroups = filteredPoints.GroupBy(p => p.Id);
            var biggestArea = pointGroups.Max(g => g.Count());
            Console.WriteLine(biggestArea.ToString());
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

        public static Point[] ProcessMap(Point[] points)
        {
            var pointsGraph = GenerateGraph(points);

            foreach (var point in pointsGraph.Keys)
            {
                var checkedPoints = new HashSet<Point>();
                var queue = new Queue<Point>(pointsGraph[point]);
                var matches = new List<Point>();
                var pointsToEnqueue = new List<Point>();
                    
                while (queue.Any())
                {
                    Point matchingPoint = queue.Dequeue();

                    if (!checkedPoints.Contains(matchingPoint))
                    {
                        if (matchingPoint.IsMaster)
                        {
                            matches.Add(matchingPoint);
                        }
                        else
                        {
                            pointsToEnqueue.AddRange(pointsGraph[matchingPoint]);
                        }
                        checkedPoints.Add(matchingPoint);
                    }

                    if (matches.Count == 0 && !queue.Any())
                    {
                        foreach (var enq in pointsToEnqueue)
                        {
                            queue.Enqueue(enq);
                        }
                        pointsToEnqueue.Clear();
                    }
                }
                if (matches.Count == 1) point.Id = matches[0].Id;
            }

            return pointsGraph.Keys.ToArray();
        }

        public static Point[] FilterPoints(Point[] processedPoints, Point[] masterPoints)
        {
            var (minX, maxX, minY, maxY) = GetMapBounds(masterPoints);

            var processedPointsList = processedPoints.ToList();
            processedPointsList.AddRange(masterPoints);
            processedPoints = processedPointsList.ToArray();
            
            var idsOnBounds = processedPoints.Where(p => p.X == minX ||
                                                         p.X == maxX ||
                                                         p.Y == minY ||
                                                         p.Y == maxY)
                                             .Select(p => p.Id)
                                             .ToHashSet();
            idsOnBounds = idsOnBounds.Distinct().ToHashSet();

            foreach (var id in idsOnBounds)
            {
                processedPoints = processedPoints.Where(p => p.Id != id).ToArray();
            }

            return processedPoints;
        }

        public static Dictionary<Point, Point[]> GenerateGraph(Point[] points)
        {
            var pointsGraph = new Dictionary<Point, Point[]>(points.Length);
            foreach (var point in points)
            {
                if (!point.IsMaster)
                {
                    var neighboursArray = points.Where(p => (p.X == point.X - 1 && p.Y == point.Y) ||
                                                            (p.X == point.X + 1 && p.Y == point.Y) ||
                                                            (p.X == point.X && p.Y == point.Y - 1) ||
                                                            (p.X == point.X && p.Y == point.Y + 1) ).ToArray();
                    pointsGraph.Add(point, neighboursArray);
                }
            }
            return pointsGraph;
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

            public int Level { get; set; } = 1;

            public bool IsMaster { get; set; } = false;
        }
    }
}
