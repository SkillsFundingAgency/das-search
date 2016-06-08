namespace Sfa.Das.Sas.Web.Models
{
    using System.Collections.Generic;

    public sealed class ProviderSearchCriteria
    {
        private string _postcode;

        public string PostCode
        {
            get { return _postcode; }
            set { _postcode = value?.Trim(); }
        }

        public int ApprenticeshipId { get; set; }

        public int Page { get; set; }

        public int Take { get; set; }

        public bool ShowAll { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }
    }
}