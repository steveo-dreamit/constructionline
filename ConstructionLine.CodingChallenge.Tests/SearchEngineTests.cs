using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        //private readonly List<Shirt> _shirts = new List<Shirt>()
        //{
        //    new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
        //    new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
        //    new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
        //    new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
        //    new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
        //    new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),

        //};

        private List<Shirt> _shirts = null;

        [SetUp]
        public void InitialiseTShirts()
        {
            _shirts = new List<Shirt>()
             {
                 new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),                 
                 new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),

                 new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                 new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),                 

                 new Shirt(Guid.NewGuid(), "Blue - Small", Size.Small, Color.Blue),                 
                 new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),                 
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
        public async System.Threading.Tasks.Task ShirtColorsGreaterThanZeroAsync(string shirtColor, string shirtSize, int expectedResult)
        {
            await IterateAllTestsAsync(shirtColor, shirtSize, expectedResult);
        }

        [Test]        
        [TestCase("Black", "Large", 0)]
        [TestCase("Yellow", "Large", 2)]
        [TestCase("Blue", "Small", 1)]
        public async System.Threading.Tasks.Task ShirtColorsMixtureGreaterThanZeroAndZeroAsync(string shirtColor, string shirtSize, int expectedResult)
        {
            await IterateAllTestsAsync(shirtColor, shirtSize, expectedResult);
        }

        [Test]
        [TestCase("Blue", 4)]
        [TestCase("Red", 3)]
        [TestCase("Tellow", 6)]
        [TestCase("Black", 2)]
        public async System.Threading.Tasks.Task CheckShirtSizesAsync(string shirtSize, int expectedResult)
        {
            await IterateAllTestsAsync(null, shirtSize, expectedResult);
        }

        [Test]
        [TestCase("Large", 5)]
        [TestCase("Small", 5)]
        [TestCase("Medium", 4)]        
        public async System.Threading.Tasks.Task CheckShirtColorsAsync(string shirtColor, int expectedResult)
        {
            await IterateAllTestsAsync(shirtColor, null, expectedResult);
        }


        //[Test]
        //public async System.Threading.Tasks.Task TestAsync()
        //{
        //    var shirts = new List<Shirt>
        //    {
        //        new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
        //        new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
        //        new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
        //    };

        //    var searchEngine = new SearchEngine(shirts);

        //    var searchOptions = new SearchOptions
        //    {
        //        Colors = new List<Color> {Color.Red},
        //        Sizes = new List<Size> {Size.Small}
        //    };

        //    var results = await searchEngine.SearchAsync(searchOptions);

        //    AssertResults(results.Shirts, searchOptions);
        //    AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
        //    AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        //}

        private async System.Threading.Tasks.Task IterateAllTestsAsync(string shirtColor, string shirtSize, int count)
        {
            var searchEngine = new SearchEngine(_shirts);

            SearchOptions searchOptions = null;

            //var searchOptions = new SearchOptions
            //{
            //    shirtColor != null ? new List<Color> { Color.All.FirstOrDefault(x => x.Name == shirtColor) } : new List<Color> { },
            //    shirtSize  != null ?  new List<Size> { Size.All.FirstOrDefault(x => x.Name == shirtSize) } : new List<Size> { }
            //};

            if (string.IsNullOrWhiteSpace(shirtColor) || string.IsNullOrWhiteSpace(shirtSize))
            {
                if (string.IsNullOrWhiteSpace(shirtColor))
                {
                    searchOptions = new SearchOptions
                    {
                        Colors = new List<Color> { },
                        Sizes = new List<Size> { Size.All.FirstOrDefault(x => x.Name == shirtSize) }
                    };
                }
                else
                {
                    searchOptions = new SearchOptions
                    {
                        Colors = new List<Color> { Color.All.FirstOrDefault(x => x.Name == shirtColor) },
                        Sizes = new List<Size> { }                       
                    };
                }
            }
            else
            {
                searchOptions = new SearchOptions
                {
                    Colors = new List<Color> { Color.All.FirstOrDefault(x => x.Name == shirtColor) },
                    Sizes = new List<Size> { Size.All.FirstOrDefault(x => x.Name == shirtSize) }
                };
            }

            var results = await searchEngine.SearchAsync(searchOptions);

            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(_shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(_shirts, searchOptions, results.ColorCounts);

            Assert.AreEqual(results.Shirts.Count, count);           
            
        }
    }
}
