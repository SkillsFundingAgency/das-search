using System.Collections.Generic;

namespace Sfa.Das.Sas.Web.ViewModels
{
    using System.Collections;

    public class ProviderStandardSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<ProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool StandardNotFound { get; set; }

        public bool PostCodeMissing { get; set; }

        public IEnumerable<DeliveryModeViewModel> DeliveryModes { get; set; }
    }
}