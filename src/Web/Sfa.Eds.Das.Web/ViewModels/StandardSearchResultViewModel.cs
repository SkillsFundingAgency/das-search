namespace Sfa.Eds.Das.Web.ViewModels
{
    using System.Collections.Generic;

    public sealed class StandardSearchResultViewModel
    {
        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<StandardResultItemViewModel> Results { get; set; }

        public bool HasError { get; set; }
    }
}