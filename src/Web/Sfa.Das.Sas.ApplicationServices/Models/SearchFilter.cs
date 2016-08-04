using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public class SearchFilter
    {
        public IEnumerable<string> DeliveryModes { get; set; }

        public SearchOption SearchOption { get; set; }
    }
}
