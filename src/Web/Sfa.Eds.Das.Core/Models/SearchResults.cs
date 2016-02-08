namespace Sfa.Eds.Das.Core.Models
{
    using System.Collections.Generic;

    public sealed class SearchResults
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<SearchResultsItem> Results { get; set; }
    }
}