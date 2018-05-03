using System.Collections.Generic;
using Sfa.Das.Sas.Core.Domain.Model;

namespace Sfa.Das.Sas.Web.ViewModels
{
    public class ProviderNameSearchResultViewModel
    {

        public long TotalResults { get; set; }

        public string SearchTerm { get; set; }

        public List<ProviderSearchResultSummary> Results { get; set; }

        public bool HasError { get; set; }
    }
}