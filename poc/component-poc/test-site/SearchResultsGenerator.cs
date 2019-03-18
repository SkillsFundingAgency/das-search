using System;
using System.Collections;
using System.Collections.Generic;
using Bogus;
using test_site.Models;

namespace test_site
{
    public class SearchResultsGenerator : IGenerateSearchResults
    {
        public IEnumerable<SearchResultItem> Generate()
        {
            var ids = 0;
            var fakeResult = new Faker<SearchResultItem>()
                .RuleFor(x => x.Id, f=> ids++)
                .RuleFor(x => x.Title, f => string.Join(" ", f.Lorem.Words()))
                .RuleFor(x => x.Level, f => f.Random.Number(1, 5))
                .RuleFor(x => x.Duration, f => f.PickRandom(new int[]{12, 24}));
            
            return fakeResult.Generate(20);
        }
    }
}