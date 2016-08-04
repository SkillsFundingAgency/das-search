using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class ProviderSearchFilter
    {
        public IEnumerable<string> DeliveryModes { get; set; }

        public ProviderFilterOptions SearchOption { get; set; }
    }
}
