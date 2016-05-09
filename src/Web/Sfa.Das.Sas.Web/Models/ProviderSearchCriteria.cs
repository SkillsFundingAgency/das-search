namespace Sfa.Das.Sas.Web.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public sealed class ProviderSearchCriteria
    {
        public string PostCode { get; set; }

        public int ApprenticeshipId { get; set; }

        public IEnumerable<string> DeliveryModes { get; set; }

        public string InputId { get; set; }
    }
}