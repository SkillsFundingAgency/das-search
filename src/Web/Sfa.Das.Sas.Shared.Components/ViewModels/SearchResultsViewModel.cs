using System.Collections.Generic;

namespace Sfa.Das.Sas.Shared.Components.ViewModels
{
    public class SearchResultsViewModel<T,TS>
    {
        public IEnumerable<T> SearchResults { get; set; }
        public long TotalResults { get; set; }
        public int LastPage { get; set; }
        public TS SearchQuery { get; set; }
    }
}