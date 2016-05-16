using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public sealed class ProviderFrameworkSearchResults
    {
        public string Title { get; set; }

        public long TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public int FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        public int Level { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<IApprenticeshipProviderSearchResultsItem> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }

        public bool FrameworkIsMissing { get; set; }

        public int FrameworkLevel { get; set; }

        public IEnumerable<string> SelectedTrainingOptions { get; set; }

        public Dictionary<string, long?> TrainingOptionsAggregation { get; set; }
    }
}