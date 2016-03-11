namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public sealed class ApprenticeshipSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<ApprenticeshipSearchResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }

    }
}