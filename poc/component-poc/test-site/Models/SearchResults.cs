using System.Collections.Generic;

namespace test_site.Models
{
    public class SearchResults
    {
        public IEnumerable<SearchResultItem> Hits { get; set; }
        public string Keywords { get; set; }
        public bool ShowSearchResults => !string.IsNullOrWhiteSpace(Keywords) && Hits != null;
        public IEnumerable<LevelAggregationViewModel> LevelAggregation { get; set; }
    }
}