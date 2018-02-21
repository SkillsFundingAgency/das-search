using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderNameSearchResultViewModel
    {

        public long TotalResults { get; set; }
        public int ActualPage { get; set; }
        public int LastPage { get; set; }
        public string SearchTerm { get; set; }
        public List<ProviderNameSearchResult> Results { get; set; }

        public bool HasError { get; set; }
        public bool ShortSearchTerm { get; set; }
    }
}