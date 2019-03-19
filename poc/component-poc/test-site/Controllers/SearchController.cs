using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using shared_lib;
using shared_lib.Models;
using test_site.Models;

namespace test_site.Controllers
{
    public class SearchController : Controller
    {
        private readonly IGenerateSearchResults _generator;

        public SearchController(IGenerateSearchResults generator) => _generator = generator;

        [Route("search", Name="default-search")]
        public IActionResult Index(string q = null, List<int> selectedLevels = null)
        {
            var model = new SearchResults() { Keywords = q };

            if (q == null) return View(model);
            
            var hits = _generator.Generate().ToList();
            model.Hits = ApplyFilter(hits, selectedLevels);
            model.LevelAggregation = BuildAggregationModels(hits);

            return View(model);
        }

        [Route("search/full-comp", Name="full-comp-search")]
        public IActionResult FullComp(string q = null, List<int> selectedLevels = null)
        {
            var model = new SearchCriteria
            {
                Keywords = q,
                SelectedLevels = selectedLevels
            };

            return View(model);
        }

        [Route("search/partial-comp", Name="partial-comp-search")]
        public IActionResult PartialComp(string q = null, List<int> selectedLevels = null)
        {
            var model = new SearchCriteria
            {
                Keywords = q,
                SelectedLevels = selectedLevels
            };
            
            return View(model);
        }

        private static IEnumerable<LevelAggregationViewModel> BuildAggregationModels(IEnumerable<SearchResultItem> hits)
        {
            return hits.GroupBy(x => x.Level)
                .Select(g => new LevelAggregationViewModel{ Value = g.Key.ToString(), Count = g.Count() })
                .OrderBy(x => x.Value);
        }

        private List<SearchResultItem> ApplyFilter(List<SearchResultItem> hits, List<int> selectedLevels)
        {
            if (selectedLevels == null || selectedLevels.Count == 0)
            return hits;

            return hits.Where(x => !selectedLevels.Contains(x.Level)).ToList();
        }
    }
}