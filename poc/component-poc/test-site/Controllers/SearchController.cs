using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            model.Hits = hits;
            model.LevelAggregation = BuildAggregationModels(hits);

            return View(model);
        }

        private static IEnumerable<LevelAggregationViewModel> BuildAggregationModels(IEnumerable<SearchResultItem> hits)
        {
            return hits.GroupBy(x => x.Level)
                .Select(g => new LevelAggregationViewModel{ Value = g.Key.ToString(), Count = g.Count() })
                .OrderBy(x => x.Value);
        }
    }
}