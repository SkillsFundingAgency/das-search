namespace Sfa.Eds.Das.Core.Models
{
    using System.Collections.Generic;

    public sealed class StandardSearchResults
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<StandardSearchResultsItem> Results { get; set; }

        public bool HasError { get; set; }
    }
}