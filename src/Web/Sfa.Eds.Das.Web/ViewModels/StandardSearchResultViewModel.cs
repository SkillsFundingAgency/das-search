namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public class StandardSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<StandardSearchResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }
    }
}