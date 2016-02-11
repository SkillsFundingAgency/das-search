namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public class ProviderSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public IEnumerable<ProviderResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }
    }
}