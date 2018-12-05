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
            string[] inputLines;
            using (var sr = new StreamReader("input.txt"))
            {
                var input = sr.ReadToEnd();
                inputLines = input.Split("\n");
            }

            var filteredPolymers = GetFilteredPolymers(inputLines[0]);
            var processedPolymers = filteredPolymers.Select(item => ProcessPolymer(item)).ToList();
            Console.WriteLine(processedPolymers.OrderBy(item => item.Length).First().Length.ToString());
        }

        public static List<string> GetFilteredPolymers(string polymer)
        {
            var filteredPolymers = new List<string>();
            char little = 'a';
            char upper = 'A';
            for (int i = 0; i <= 25; i++)
            {
                var filteredPolymer = polymer.Replace(new string(new char[] {(char)(little + i)}), string.Empty)
                                             .Replace(new string(new char[] {(char)(upper + i)}), string.Empty);
                filteredPolymers.Add(filteredPolymer);
            }
            
            return filteredPolymers;
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
