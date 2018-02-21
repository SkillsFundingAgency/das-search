using System.Collections.Generic;

namespace Sfa.Das.Sas.Core.Domain.Model
{

    public class ProviderSearchResult
    {
        public int TotalCount { get; set; }
        public List<ProviderNameSearchResult> Providers { get; set; }
    }
}
