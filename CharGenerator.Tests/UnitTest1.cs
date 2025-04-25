using generator;

namespace CharGenerator.Tests
{
    public class UnitTest1
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

        [Fact]
        public void BigramsGenerator_GetSym_ReturnsValidBigram()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            string result = generator.getSym();
            Assert.Equal(result.Length, 2);
        }

        [Fact]
        public void BigramsGenerator_GenerateText_ReturnsCorrectLength()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            string text = generator.GenerateText(5);
            Assert.Equal(10, text.Length);
        }

        [Fact]
        public void WordsGenerator_GetSym_ReturnsValidWord()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            string result = generator.getSym();
            Assert.True(result.Length >= 2);
        }

        [Fact]
        public void WordsGenerator_GenerateText_ContainsWords()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            string text = generator.GenerateText(3);
            Assert.True(text.Split(" ").Length >= 3);
        }

        [Fact]
        public void BigramsGenerator_GetWeight_ReturnsZeroForUnknownBigram()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            Assert.Equal(0, generator.GetWeight("unknown"));
        }

        [Fact]
        public void WordsGenerator_GetWeight_ReturnsZeroForUnknownWord()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            Assert.Equal(0, generator.GetWeight("unknown"));
        }

        [Fact]
        public void BigramsGenerator_GenerateText_ContainsOnlyValidBigrams()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            string text = generator.GenerateText(10);
            for (int i = 0; i < text.Length; i += 2)
            {
                Assert.True(text.Substring(i, 2).All(char.IsLetter));
            }
        }

        [Fact]
        public void WordsGenerator_GenerateText_ContainsOnlyLetters()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            string text = generator.GenerateText(5);
            Assert.True(text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
        }

        [Fact]
        public void BigramsGenerator_GetSym_ReturnsDifferentValues()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            var results = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                results.Add(generator.getSym());
            }
            Assert.True(results.Count > 1);
        }

        [Fact]
        public void WordsGenerator_GetSym_ReturnsDifferentWords()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            var results = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                results.Add(generator.getSym());
            }
            Assert.True(results.Count > 1);
        }

        [Fact]
        public void BigramsGenerator_GenerateText_ReturnsDifferentTexts()
        {
            var generator = new BigramsGenerator();
            generator.Load(projectDirectory);
            var text1 = generator.GenerateText(10);
            var text2 = generator.GenerateText(10);
            Assert.NotEqual(text1, text2);
        }

        [Fact]
        public void WordsGenerator_GenerateText_ReturnsDifferentTexts()
        {
            var generator = new WordsGenerator();
            generator.Load(projectDirectory);
            var text1 = generator.GenerateText(5);
            var text2 = generator.GenerateText(5);
            Assert.NotEqual(text1, text2);
        }
    }
}