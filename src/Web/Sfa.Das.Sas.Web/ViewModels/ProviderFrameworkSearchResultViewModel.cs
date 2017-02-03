using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderFrameworkSearchResultViewModel
    {
        public string Title { get; set; }

        public long TotalResults { get; set; }

        public long TotalProvidersCountry { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public string FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<FrameworkProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }

        public int FrameworkLevel { get; set; }

        public IEnumerable<DeliveryModeViewModel> DeliveryModes { get; set; }

        public NationalProviderViewModel NationalProviders { get; set; }

        public string SearchTerms { get; set; }

        public string AbsolutePath { get; set; }

        public bool ShowAll { get; set; }

        public bool ShowNationalProviders { get; set; }

        public bool IsLevyPayingEmployer { get; set; }
    }
}