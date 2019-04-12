using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class FatSearchResultsViewModel
    { 
        public IEnumerable<FatSearchResultsItemViewModel> SearchResults { get; set; }
        public long TotalResults { get; set; }
        public int LastPage { get; set; }
        public SearchQueryViewModel SearchQuery { get; set; } = new SearchQueryViewModel();
    }
}
