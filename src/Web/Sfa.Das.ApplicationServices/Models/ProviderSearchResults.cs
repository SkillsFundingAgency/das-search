namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    using Sfa.Eds.Das.Core.Domain;

    public sealed class ProviderSearchResults
    {
        public long TotalResults { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public IEnumerable<ProviderSearchResultsItem> Results { get; set; }

        public bool HasError { get; set; }
    }
}