using System.Collections.Generic;
using shared_lib.Models;

namespace component_lib
{
    public class SearchFilterModel
    {
        public string SearchRouteName { get; set; }
        public bool ShowSearchFilter => !string.IsNullOrWhiteSpace(Keywords) && LevelAggregation != null;
        public string Keywords { get; internal set; }
        public IEnumerable<LevelAggregationViewModel> LevelAggregation { get; internal set; }
    }
}