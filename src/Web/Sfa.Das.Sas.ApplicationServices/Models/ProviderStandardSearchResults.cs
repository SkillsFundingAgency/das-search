using System.Collections.Generic;

namespace Sfa.Das.Sas.ApplicationServices.Models
{
    public sealed class ProviderStandardSearchResults : BaseProviderSearchResults
    {
        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string StandardResponseCode { get; set; }
    }
}