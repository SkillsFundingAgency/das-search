namespace Sfa.Das.Sas.Web.Models
{
    using System.Collections.Generic;

    public sealed class ProviderSearchCriteria
    {
        public string PostCode { get; set; }

        public int ApprenticeshipId { get; set; }

        public int Page { get; set; }

        public int Take { get; set; }

        public bool ShowAll { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }
    }
}