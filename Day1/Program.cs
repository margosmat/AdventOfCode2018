using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sr = new StreamReader("input.txt"))
            {
                var line = sr.ReadToEnd();
                var stringCollection = line.Split("\n");
                var numbers = stringCollection.Where(number => !string.IsNullOrWhiteSpace(number)).Select(number => Int32.Parse(number)).ToArray();
                int frequency = 0;
                List<int> frequencyCache = new List<int> {0};
                var numbersEnumerator = numbers.GetEnumerator();
                numbersEnumerator.MoveNext();
                int duplicateFrequency = int.MaxValue;
                while (duplicateFrequency == int.MaxValue)
                {
                    frequency += (int)numbersEnumerator.Current;
                    if (frequencyCache.Any(cachedFrequency => cachedFrequency == frequency)) 
                    {
                        duplicateFrequency = frequency;
                        Console.WriteLine(duplicateFrequency.ToString());
                        Console.WriteLine(frequencyCache.ToString());
                        return;
                    }
                    if(!numbersEnumerator.MoveNext()) 
                    {
                        numbersEnumerator.Reset();
                        numbersEnumerator.MoveNext();
                    }
                    frequencyCache.Add(frequency);
                }
                Console.WriteLine(frequency.ToString());
            }
        }
    }
}
