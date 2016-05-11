using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    using System.Collections;

    public class ProviderStandardSearchResultViewModel
    {
        public double TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int ActualPage { get; set; }

        public int LastPage { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<ProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }

        public IEnumerable<DeliveryModeViewModel> DeliveryModes { get; set; }
    }
}