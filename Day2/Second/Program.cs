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
            using (var sr = new StreamReader("input.txt"))
            {
                var line = sr.ReadToEnd();
                var stringCollection = line.Split("\n");
                var answer = GetTwoSimilarLabels(stringCollection);
                Console.WriteLine(answer);
            }
        }

        static (string, string) GetTwoSimilarLabels(string[] collection)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                for (int j = 0; j < collection.Length; j++)
                {
                    int count = 0;
                    if (i != j && collection[i].Length == collection[j].Length)
                    {
                        for (int k = 0; k < collection[i].Length; k++)
                        {
                            if (collection[i][k] == collection[j][k]) count++;
                        }
                        if (count == collection[i].Length - 1) return (collection[i], collection[j]);
                        count = 0;
                    }
                }
            }

            return ("", "");
        }
    }
}
