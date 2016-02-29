namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    public sealed class ProviderSearchResults
    {
        public long TotalResults { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<ProviderSearchResultsItem> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }
    }
}