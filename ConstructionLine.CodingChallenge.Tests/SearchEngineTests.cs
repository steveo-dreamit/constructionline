using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        private List<Shirt> _shirts = null;

        [SetUp]
        public void InitialiseTShirts()
        {
            _shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red),
                new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),

                new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Black - Large", Size.Large, Color.Black),

                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),

                new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Yellow - Medium", Size.Medium, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Yellow - Medium", Size.Medium, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow)
            };
        }

        [Test]
        [TestCase("Red", "Small", 1)]
        [TestCase("Black", "Medium", 1)]
        [TestCase("Blue", "Large", 2)]
        [TestCase("Yellow", "Small", 2)]
        public async System.Threading.Tasks.Task ShirtColorsAsync(string shirtColor, string shirtSize, int expectedResult)
        {
            // var searchEngine = new SearchEngine(_shirts);

            await IterateAllTestsAsync(shirtColor, shirtSize, expectedResult);
        }


        [Test]
        public async System.Threading.Tasks.Task TestAsync()
        {
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red},
                Sizes = new List<Size> {Size.Small}
            };

            var results = await searchEngine.SearchAsync(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        private async System.Threading.Tasks.Task IterateAllTestsAsync(string shirtColor, string shirtSize, int count)
        {
            var searchEngine = new SearchEngine(_shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> { Color.All.FirstOrDefault(x => x.Name == shirtColor) },
                Sizes = new List<Size> { Size.All.FirstOrDefault(x => x.Name == shirtSize) }
            };

            var results = await searchEngine.SearchAsync(searchOptions);

            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);

            Assert.AreEqual(results.Shirts.Count, count);           
            
        }
    }
}
