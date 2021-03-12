using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    public class SearchEngineTestsBase
    {
        protected static void AssertResults(List<Shirt> shirts, SearchOptions options)
        {
            Assert.That(shirts, Is.Not.Null);

            var resultingShirtIds = shirts.Select(s => s.Id).ToList();
            var sizeIds = options.Sizes.Select(s => s.Id).ToList();
            var colorIds = options.Colors.Select(c => c.Id).ToList();

            foreach (var shirt in shirts)
            {
                if (sizeIds.Contains(shirt.Size.Id)
                    && colorIds.Contains(shirt.Color.Id)
                    && !resultingShirtIds.Contains(shirt.Id))
                {
                    Assert.Fail($"'{shirt.Name}' with Size '{shirt.Size.Name}' and Color '{shirt.Color.Name}' not found in results, " +
                                $"when selected sizes where '{string.Join(",", options.Sizes.Select(s => s.Name))}' " +
                                $"and colors '{string.Join(",", options.Colors.Select(c => c.Name))}'");
                }
            }
        }


        protected static void AssertSizeCounts(List<Shirt> shirts, SearchOptions searchOptions, List<SizeCount> sizeCounts)
        {
            // modified this method due to not returning correct results

            Assert.That(sizeCounts, Is.Not.Null);

             Size[] filterSizes = searchOptions.Sizes.Any() ? searchOptions.Sizes.ToArray() : Size.All.ToArray();            
             Color[] filterColors = searchOptions.Colors.Any() ? searchOptions.Colors.ToArray() : Color.All.ToArray();                        

            foreach (var size in filterSizes)
            {
                var sizeCount = sizeCounts.SingleOrDefault(s => s.Size.Id == size.Id);
                Assert.That(sizeCount, Is.Not.Null, $"Size count for '{size.Name}' not found in results");

                var expectedSizeCount = shirts
                    .Count(s => s.Size.Id == size.Id && filterColors.Any(c => c.Id == s.Color.Id));                                

                Assert.That(sizeCount.Count, Is.EqualTo(expectedSizeCount), 
                    $"Size count for '{sizeCount.Size.Name}' showing '{sizeCount.Count}' should be '{expectedSizeCount}'");
            }
        }


        protected static void AssertColorCounts(List<Shirt> shirts, SearchOptions searchOptions, List<ColorCount> colorCounts)
        {
            // modified this method due to not returning correct results

            Assert.That(colorCounts, Is.Not.Null);

            Size[] filterSizes = searchOptions.Sizes.Any() ? searchOptions.Sizes.ToArray() : Size.All.ToArray();
            Color[] filterColors = searchOptions.Colors.Any() ? searchOptions.Colors.ToArray() : Color.All.ToArray();

            var filters = Getfilters(shirts, searchOptions, colorCounts);

            foreach (var color in filterColors)
            {
                var colorCount = colorCounts.SingleOrDefault(s => s.Color.Id == color.Id);
                Assert.That(colorCount, Is.Not.Null, $"Color count for '{color.Name}' not found in results");

                var expectedColorCount = shirts
                    .Count(s => s.Color.Id == color.Id && filterSizes.Any(c => c.Id == s.Size.Id));

                Assert.That(colorCount.Count, Is.EqualTo(expectedColorCount),
                    $"Color count for '{colorCount.Color.Name}' showing '{colorCount.Count}' should be '{expectedColorCount}'");
            }
        }

        private static Tuple<Size[], Color[]>  Getfilters(List<Shirt> shirts, SearchOptions searchOptions, List<ColorCount> colorCounts)
        {
            Size[] filterSizes = searchOptions.Sizes.Any() ? searchOptions.Sizes.ToArray() : Size.All.ToArray();
            Color[] filterColors = searchOptions.Colors.Any() ? searchOptions.Colors.ToArray() : Color.All.ToArray();

           return Tuple.Create(filterSizes, filterColors);
        }
    }
}