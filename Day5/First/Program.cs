using System;
using System.IO;

namespace First
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] inputLines;
            using (var sr = new StreamReader("input.txt"))
            {
                var input = sr.ReadToEnd();
                inputLines = input.Split("\n");
            }

            var processedPolymer = ProcessPolymer(inputLines[0]);
            Console.WriteLine(processedPolymer.Length.ToString());
        }

        public static string ProcessPolymer(string polymer)
        {
            bool polymerChanged = true;
            string currentPolymer = polymer;

            while (polymerChanged)
            {
                int initialCharCount = currentPolymer.Length;
                char little = 'a';
                char upper = 'A';
                for (int i = 0; i <= 25; i++)
                {
                    bool innerPolymerChanged = true;
                    while (innerPolymerChanged)
                    {
                        int polymerCharCounts = currentPolymer.Length;
                        var match = new string(new char[] {(char)(little + i), (char)(upper + i)});
                        currentPolymer = currentPolymer.Replace(match, string.Empty);
                        match = new string(new char[] {(char)(upper + i), (char)(little + i)});
                        currentPolymer = currentPolymer.Replace(match, string.Empty);
                        innerPolymerChanged = polymerCharCounts != currentPolymer.Length;
                    }
                }
                polymerChanged = initialCharCount != currentPolymer.Length;
            }

            return currentPolymer;
        }
    }
}
