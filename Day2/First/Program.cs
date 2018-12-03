using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = new StreamReader("input.txt"))
            {
                var line = sr.ReadToEnd();
                var stringCollection = line.Split("\n");
                var doubleLetterLabels = GetIDNumberForDoubleLetters(stringCollection);
                var tripleLetterLabels = GetIDNumberForTripleLetters(stringCollection);
                var checksum = doubleLetterLabels.Count * tripleLetterLabels.Count;
                Console.WriteLine(checksum.ToString());
            }
        }

        static List<string> GetIDNumberForDoubleLetters(string[] collection)
        {
            List<string> labelsWithDoubleLetters = new List<string>();
            foreach(var label in collection)
            {
                List<char> charCollection = new List<char>();
                foreach(var character in label)
                {
                    if (!charCollection.Contains(character)) charCollection.Add(character);
                }
                Dictionary<char, int> charCount = new Dictionary<char, int>();
                foreach(var uniqueChar in charCollection)
                {
                    int count = 0;
                    for (int i = 0; i < label.Length; i++)
                    {
                        if (label[i] == uniqueChar) count++;
                    }
                    charCount[uniqueChar] = count;
                }
                if (charCount.Values.Any(i => i == 2)) labelsWithDoubleLetters.Add(label);
            }
            return labelsWithDoubleLetters;
        }

        static List<string> GetIDNumberForTripleLetters(string[] collection)
        {
            List<string> labelsWithTripleLetters = new List<string>();
            foreach(var label in collection)
            {
                List<char> charCollection = new List<char>();
                foreach(var character in label)
                {
                    if (!charCollection.Contains(character)) charCollection.Add(character);
                }
                Dictionary<char, int> charCount = new Dictionary<char, int>();
                foreach(var uniqueChar in charCollection)
                {
                    int count = 0;
                    for (int i = 0; i < label.Length; i++)
                    {
                        if (label[i] == uniqueChar) count++;
                    }
                    charCount[uniqueChar] = count;
                }
                if (charCount.Values.Any(i => i == 3)) labelsWithTripleLetters.Add(label);
            }
            return labelsWithTripleLetters;
        }
    }
}
