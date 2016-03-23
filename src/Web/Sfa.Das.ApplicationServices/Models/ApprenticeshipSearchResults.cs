namespace Sfa.Das.ApplicationServices.Models
{
    using System.Collections.Generic;

    public sealed class ApprenticeshipSearchResults
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<ApprenticeshipSearchResultsItem> Results { get; set; }

        public bool HasError { get; set; }
    }
}