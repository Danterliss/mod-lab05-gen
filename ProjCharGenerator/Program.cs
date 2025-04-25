using System;
using System.Collections.Generic;
using System.IO;
using ScottPlot;
using System.Drawing;
using System.Linq;

namespace generator
{
    public class BigramsGenerator
    {
        private List<(string bigram, int weight)> bigrams = new();
        private Random random = new();
        private int totalWeight;
        SortedDictionary<string, int> statistics = new();

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
                string bigram = getSym();
                if (statistics.ContainsKey(bigram))
                    statistics[bigram]++;
                else
                    statistics.Add(bigram, 1);
                result += bigram;
            }
            return result;
        }

        private void SaveToFile(string text, string projectDirectory)
        {
            string solutionRoot = Directory.GetParent(projectDirectory).FullName;
            string resultsDir = Path.Combine(solutionRoot, "Results");
            Directory.CreateDirectory(resultsDir);
            string filePath = Path.Combine(resultsDir, "gen-1.txt");
            File.WriteAllText(filePath, text);
        }

        public void CreatePlot(string projectDirectory)
        {
            if (statistics.Count == 0)
            {
                Console.WriteLine("Нет данных для построения графика");
                return;
            }

            var plot = new Plot();
            int barsCount = Math.Min(statistics.Count, 100);
            int k = 1;

            for (int i = 0; i < barsCount; i++)
            {
                var item = statistics.ElementAt(i);
                double[] positions = { k + 1, k + 1.8 };
                double[] values = { item.Value / 1000.0, (double)GetWeight(item.Key) / totalWeight };

                var bars = plot.Add.Bars(positions, values);
                bars.Bars[0].FillColor = Colors.Green;
                bars.Bars[1].FillColor = Colors.Red; 
                k += 4;
            }

            var tickGen = new ScottPlot.TickGenerators.NumericManual();
            int j = 0;
            for (int x = 1; x <= 4 * barsCount; x += 4)
            {
                tickGen.AddMajor(x + 1.5, statistics.ElementAt(j).Key);
                j++;
            }
            plot.Axes.Bottom.TickGenerator = tickGen;

            var legendItems = new[]
            {
                new LegendItem
                {
                    LabelText = "Actual frequency",
                    LineWidth = 10,
                    LineColor = Colors.Green,
                    MarkerShape = MarkerShape.None 
                },
                new LegendItem
                {
                    LabelText = "Expected frequency",
                    LineWidth = 10,
                    LineColor = Colors.Red,
                    MarkerShape = MarkerShape.None
                }
            };

            plot.ShowLegend(legendItems);
            plot.Legend.Alignment = Alignment.UpperRight;

            plot.Grid.XAxisStyle.IsVisible = false;
            plot.Axes.Margins(bottom: 0);
            plot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            plot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
            plot.YLabel("Frequency");

            string solutionRoot = Directory.GetParent(projectDirectory).FullName;
            string resultsDir = Path.Combine(solutionRoot, "Results");
            Directory.CreateDirectory(resultsDir);
            string filePath = Path.Combine(resultsDir, "gen-1.png");
            plot.SavePng(filePath, 2000, 500);
        }

        private int GetWeight(string bigram)
        {
            return bigrams.Find(x => x.bigram == bigram).weight;
        }

        public void BigramsText(string projectDirectory)
        {
            Load(projectDirectory);
            string text = GenerateText(1000);
            SaveToFile(text, projectDirectory);
            CreatePlot(projectDirectory);
        }
    }

    public class WordsGenerator
    {
        private List<(string word, float weight)> words = new();
        private Random random = new();
        private float totalWeight;
        SortedDictionary<string, int> statistics = new();

        private void Load(string projectDirectory)
        {
            string inputFilePath = Path.Combine(projectDirectory, "words.txt");
            foreach (var line in File.ReadLines(inputFilePath))
            {
                var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3 && float.TryParse(parts[2], out float weight))
                {
                    words.Add((parts[1], weight));
                    totalWeight += weight;
                }
            }
        }

        private string getSym()
        {
            if (words.Count == 0) return "";

            int randomValue = random.Next((int)totalWeight);
            float sum = 0;

            foreach (var (words, weight) in words)
            {
                sum += weight;
                if (randomValue < sum)
                    return words;
            }

            return words[0].word;
        }

        private string GenerateText(int length)
        {
            string result = "";
            for (int i = 0; i < length; i++)
            {
                string word = getSym();
                if (statistics.ContainsKey(word))
                    statistics[word]++;
                else
                    statistics.Add(word, 1);
                result += word;
            }
            return result;
        }

        private void SaveToFile(string text, string projectDirectory)
        {
            string solutionRoot = Directory.GetParent(projectDirectory).FullName;
            string resultsDir = Path.Combine(solutionRoot, "Results");
            Directory.CreateDirectory(resultsDir);
            string filePath = Path.Combine(resultsDir, "gen-2.txt");
            File.WriteAllText(filePath, text);
        }

        public void CreatePlot(string projectDirectory)
        {
            if (statistics.Count == 0)
            {
                Console.WriteLine("Нет данных для построения графика");
                return;
            }

            var plot = new Plot();
            int barsCount = Math.Min(statistics.Count, 100);
            int k = 1;

            for (int i = 0; i < barsCount; i++)
            {
                var item = statistics.ElementAt(i);
                double[] positions = { k + 1, k + 1.8 };
                double[] values = { item.Value / 1000.0, (double)GetWeight(item.Key) / totalWeight };

                var bars = plot.Add.Bars(positions, values);
                bars.Bars[0].FillColor = Colors.Green;
                bars.Bars[1].FillColor = Colors.Red; 
                k += 4;
            }

            var tickGen = new ScottPlot.TickGenerators.NumericManual();
            int j = 0;
            for (int x = 1; x <= 4 * barsCount; x += 4)
            {
                tickGen.AddMajor(x + 1.5, statistics.ElementAt(j).Key);
                j++;
            }
            plot.Axes.Bottom.TickGenerator = tickGen;

            var legendItems = new[]
            {
                new LegendItem
                {
                    LabelText = "Actual frequency",
                    LineWidth = 10,
                    LineColor = Colors.Green,
                    MarkerShape = MarkerShape.None 
                },
                new LegendItem
                {
                    LabelText = "Expected frequency",
                    LineWidth = 10,
                    LineColor = Colors.Red,
                    MarkerShape = MarkerShape.None
                }
            };

            plot.ShowLegend(legendItems);
            plot.Legend.Alignment = Alignment.UpperRight;

            plot.Grid.XAxisStyle.IsVisible = false;
            plot.Axes.Margins(bottom: 0);
            plot.Axes.Bottom.TickLabelStyle.Rotation = -45;
            plot.Axes.Bottom.TickLabelStyle.Alignment = Alignment.MiddleRight;
            plot.YLabel("Frequency");

            string solutionRoot = Directory.GetParent(projectDirectory).FullName;
            string resultsDir = Path.Combine(solutionRoot, "Results");
            Directory.CreateDirectory(resultsDir);
            string filePath = Path.Combine(resultsDir, "gen-2.png");
            plot.SavePng(filePath, 2000, 500);
        }

        private int GetWeight(string word)
        {
            return (int)words.Find(x => x.word == word).weight;
        }

        public void WordsText(string projectDirectory)
        {
            Load(projectDirectory);
            string text = GenerateText(1000);
            SaveToFile(text, projectDirectory);
            CreatePlot(projectDirectory);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            var generatorB = new BigramsGenerator();
            generatorB.BigramsText(projectDirectory);

            var generatorW = new WordsGenerator();
            generatorW.WordsText(projectDirectory);
        }
    }
}

