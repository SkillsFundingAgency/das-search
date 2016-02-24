namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public class ProviderSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public int StandardId { get; set; }

        public string StandardName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<ProviderResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }
    }
}