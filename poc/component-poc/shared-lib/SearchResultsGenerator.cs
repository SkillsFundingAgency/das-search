using System;
using System.Collections;
using System.Collections.Generic;
using Bogus;
using Microsoft.Extensions.Logging;
using shared_lib.Models;

namespace shared_lib
{
    public class SearchResultsGenerator : IGenerateSearchResults
    {
        private readonly ILogger<SearchResultsGenerator> _logger;
        private IEnumerable<SearchResultItem> _results;

        public SearchResultsGenerator(ILogger<SearchResultsGenerator> logger)
        {
            _logger = logger;
        }

        public IEnumerable<SearchResultItem> Generate()
        {
            if (_results == null)
            {
                _logger.LogDebug("Building Search results");
                _results = BuildFakeResults();

                return _results;
            }
            else 
            {
                _logger.LogDebug("Using cached copy");
                return _results;
            }
        }

        private IEnumerable<SearchResultItem> BuildFakeResults()
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