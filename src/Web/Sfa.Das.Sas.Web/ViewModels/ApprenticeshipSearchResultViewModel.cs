using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public sealed class ApprenticeshipSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public string SearchTerm { get; set; }

        public string SortOrder { get; set; }

        public IEnumerable<ApprenticeshipSearchResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }

        public IEnumerable<LevelAggregationViewModel> AggregationLevel { get; set; }
        public Dictionary<int, bool> ShortlistedStandards { get; set; }
        public Dictionary<int, bool> ShortlistedFrameworks { get; set; }
    }
}