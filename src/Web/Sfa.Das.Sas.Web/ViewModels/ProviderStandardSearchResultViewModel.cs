using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderStandardSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public string SortOrder { get; set; }

        public string Ukprn { get; set; }

        public string StandardId { get; set; }

        public string StandardName { get; set; }

        public object StandardLevel { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<StandardProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }

        public long TotalResultsForCountry { get; set; }

        public IEnumerable<DeliveryModeViewModel> DeliveryModes { get; set; }

        public NationalProviderViewModel NationalProviders { get; set; }

        public string SearchTerms { get; set; }

        public string AbsolutePath { get; set; }

        public bool ShowAll { get; set; }

        public bool ShowNationalProviders { get; set; }

        public bool IsLevyPayingEmployer { get; set; }

        public ManageApprenticeshipFundsViewModel ManageApprenticeshipFunds
        {
            get;
            set;
        }
    }
}