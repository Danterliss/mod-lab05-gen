using System;
using System.Collections.Generic;
using System.IO;

namespace generator
{
    public class BigramsGenerator
    {
        private List<(string bigram, int weight)> bigrams = new();
        private Random random = new();
        private int totalWeight;

        private void Load(string projectDirectory)
        {
            string inputFilePath = Path.Combine(projectDirectory, "bigrams.txt");
            foreach (var line in File.ReadLines(inputFilePath))
            {
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3 && int.TryParse(parts[2], out int weight))
                {
                    bigrams.Add((parts[1], weight));
                    totalWeight += weight;
                }
            }
        }

        private string getSym()
        {
            if (bigrams.Count == 0) return "";

            int randomValue = random.Next(totalWeight);
            int sum = 0;

            foreach (var (bigram, weight) in bigrams)
            {
                sum += weight;
                if (randomValue < sum)
                    return bigram;
            }

            return bigrams[0].bigram;
        }

        private string GenerateText(int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += getSym();
            }
            return result;
        }

        private void SaveToFile(string text, string projectDirectory)
        {
            string solutionRoot = Directory.GetParent(projectDirectory).FullName;
            string resultsDir = Path.Combine(solutionRoot, "Results");
            Directory.CreateDirectory(resultsDir);
            string filePath = Path.Combine(resultsDir, "gen-1.txt");
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, text);
            else
                File.WriteAllText(filePath, text);
        }

        public void BigramsText(string projectDirectory)
        {
            Load(projectDirectory);
            string text = GenerateText(1000);
            SaveToFile(text, projectDirectory);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            var generator = new BigramsGenerator();
            generator.BigramsText(projectDirectory);

        }
    }
}

