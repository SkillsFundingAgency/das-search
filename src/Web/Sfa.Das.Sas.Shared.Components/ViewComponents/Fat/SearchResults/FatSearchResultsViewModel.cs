using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class FatSearchResultsViewModel
    {
        public string Keywords { get; set; }
        public ICssClasses CssClasses { get; set; }
        public IEnumerable<FatSearchResultsItemViewModel> SearchResults { get; set; }
        public long TotalResults { get; set; }

        public int ResultsToTake { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }

        public string SortOrder { get; set; }
    }
}
