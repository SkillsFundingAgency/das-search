using System.Collections.Generic;
using shared_lib.Models;

namespace component_lib
{
    public class FullPageSearch
    {
        public string SearchRouteName { get; set; }
        public IEnumerable<SearchResultItem> Hits { get; internal set; }
        public bool ShowSearchResults => !string.IsNullOrWhiteSpace(Keywords) && Hits != null;

        public string Keywords { get; internal set; }
        public IEnumerable<LevelAggregationViewModel> LevelAggregation { get; internal set; }
    }
}