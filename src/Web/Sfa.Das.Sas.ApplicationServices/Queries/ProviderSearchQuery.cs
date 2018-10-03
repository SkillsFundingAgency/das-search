using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Queries
{
    public class ProviderSearchQuery
    {
        private string _postcode;

        public string PostCode
        {
            get { return _postcode; }
            set { _postcode = value?.Trim(); }
        }

        public bool IsLevyPayingEmployer { get; set; }

        public int Page { get; set; }

        public bool NationalProvidersOnly { get; set; }

        public bool ShowAll { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }

        public string Keywords { get; set; }

        public string ApprenticeshipId { get; set; }
        public string Ukprn { get; set; }
    }
}
