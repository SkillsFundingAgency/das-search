namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public class ProviderFrameworkSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public int FrameworkId { get; set; }

        public int FrameworkCode { get; set; }

        public string FrameworkName { get; set; }

        public string PathwayName { get; set; }

        public string PostCode { get; set; }

        public IEnumerable<FrameworkProviderResultItemViewModel> Hits { get; set; }

        public bool HasError { get; set; }

        public bool PostCodeMissing { get; set; }
    }
}