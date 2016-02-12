namespace Sfa.Eds.Das.Core.Models
{
    using System.Collections.Generic;

    public sealed class ProviderSearchResults
    {
        public long TotalResults { get; set; }

        public string StandardName { get; set; }

        public IEnumerable<ProviderSearchResultsItem> Results { get; set; }

        public bool HasError { get; set; }
    }
}