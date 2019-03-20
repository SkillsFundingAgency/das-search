using System.Collections.Generic;
using shared_lib.Models;

namespace component_lib
{
    public class SearchResultsModel
    {
        public IEnumerable<SearchResultItem> Hits { get; internal set; }
        public bool ShowSearchResults => !string.IsNullOrWhiteSpace(Keywords) && Hits != null;
        public string Keywords { get; internal set; }
    }
}