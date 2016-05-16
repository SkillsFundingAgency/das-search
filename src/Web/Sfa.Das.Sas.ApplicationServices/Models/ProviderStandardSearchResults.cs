using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public sealed class ProviderStandardSearchResults
    {
        public long TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<IApprenticeshipProviderSearchResultsItem> Hits { get; set; }

        public bool HasError { get; set; }

        public bool StandardNotFound { get; set; }

        public bool PostCodeMissing { get; set; }

        public Dictionary<string, long?> TrainingOptionsAggregation { get; set; }

        public IEnumerable<string> SelectedTrainingOptions { get; set; }
    }
}