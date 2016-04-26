using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public sealed class ApprenticeshipSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<ApprenticeshipSearchResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }
    }
}