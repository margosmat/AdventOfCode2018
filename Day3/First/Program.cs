using System;
using System.IO;

namespace First
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = new StreamReader("input.txt"))
            {
                var line = sr.ReadToEnd();
                var stringCollection = line.Split("\n");
                int[,] fabricMatrix = new int[1000,1000];
                foreach (var fabric in stringCollection)
                {
                    if (!string.IsNullOrWhiteSpace(fabric))
                    {
                        var spaceDividedSubstrings = fabric.Split(' ');
                        var fabricPosition = spaceDividedSubstrings[2].Replace(':', ' ').Split(',');
                        int fabricPositionX = Int32.Parse(fabricPosition[0]);
                        int fabricPositionY = Int32.Parse(fabricPosition[1]);
                        var fabricSizes = spaceDividedSubstrings[3].Split('x');
                        int fabricWidth = Int32.Parse(fabricSizes[0]);
                        int fabricHeight = Int32.Parse(fabricSizes[1]);

                        for (int i = fabricPositionX - 1; i < fabricPositionX - 1 + fabricWidth; i++)
                        {
                            for (int j = fabricPositionY - 1; j < fabricPositionY - 1 + fabricHeight; j++)
                            {
                                fabricMatrix[i,j]++;
                            }
                        }
                    }
                }

                int multipleClaims = 0;

                for (int i = 0; i < 1000; i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        if (fabricMatrix[i,j] > 1) multipleClaims++;
                    }
                }
                Console.WriteLine(multipleClaims.ToString());
            }
        }
    }
}
