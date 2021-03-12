using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {        
        private readonly IDictionary<Guid, Shirt[]> _shirtSizes;
        private readonly IDictionary<Guid, Shirt[]> _shirtColors;

        public SearchEngine(List<Shirt> shirts)
        {
            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.
            _shirtSizes = shirts.GroupBy(x => x.Size.Id).ToDictionary(x => x.Key, x => x.ToArray());
            _shirtColors = shirts.GroupBy(x => x.Color.Id).ToDictionary(x => x.Key, x => x.ToArray());
        }

        public async System.Threading.Tasks.Task<SearchResults> SearchAsync(SearchOptions options)
        {
            // TODO: search logic goes here.
            var colorSearch = await SearchByColor(options.Colors.Select(x => x.Id).ToArray());

            var sizeSearch = await SearchBySize(options.Sizes.Select(x => x.Id).ToArray());

            var intermediateResults = colorSearch.Intersect(sizeSearch).ToArray();

            var resultColor = await Colours(intermediateResults);

            var resultSize = await Sizes(intermediateResults);            

            return new SearchResults
            {
                Shirts = intermediateResults.ToList(),
                ColorCounts = resultColor,
                SizeCounts = resultSize
            };
        }        

        private Task<Shirt[]> SearchBySize(Guid[] sizes)
        {
            var filter = sizes.Any() ? sizes : Size.All.Select(x => x.Id).ToArray();
            var result = new List<Shirt>();

            foreach (var f in filter)
            {
                if (_shirtSizes.TryGetValue(f, out Shirt[] shirts))
                {
                    result.AddRange(shirts);
                }
            }

            return Task.FromResult(result.ToArray());
        }

        private Task<Shirt[]> SearchByColor(Guid[] colors)
        {
            var filter = colors.Any() ? colors : Color.All.Select(x => x.Id).ToArray();
            var result = new List<Shirt>();

            foreach( var f in filter)
            {
                if (_shirtColors.TryGetValue(f, out Shirt[] shirts))
                {
                    result.AddRange(shirts);
                }                
            }

            return Task.FromResult(result.ToArray());
        }

        private Task<List<ColorCount>> Colours(Shirt[] results)
        {
            var Summary = results.GroupBy(x => x.Color).Select(c => new ColorCount()
            {
                Color = c.Key,
                Count = c.Count()
            }).ToList();

            foreach(var result in Color.All.Except(Summary.Select(x=>x.Color)))
            {
                Summary.Add(new ColorCount() { Color = result, Count = 0 });
            }

            return Task.FromResult(Summary);
        }

        private Task<List<SizeCount>> Sizes(Shirt[] results)
        {
            var Summary = results.GroupBy(x => x.Size).Select(c => new SizeCount()
            {
                Size = c.Key,
                Count = c.Count()
            }).ToList();

            foreach (var result in Size.All.Except(Summary.Select(x => x.Size)))
            {
                Summary.Add(new SizeCount() { Size = result, Count = 0 });
            }

            return Task.FromResult(Summary);
        }
    }
}