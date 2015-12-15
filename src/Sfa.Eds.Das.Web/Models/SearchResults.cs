using System.Collections.Generic;

namespace Sfa.Eds.Das.Web.Models
{
    public sealed class SearchResults
    {
        public int TotalResults { get; set; }

        public IEnumerable<SearchResultsItem> Results { get; set; }
    }
}