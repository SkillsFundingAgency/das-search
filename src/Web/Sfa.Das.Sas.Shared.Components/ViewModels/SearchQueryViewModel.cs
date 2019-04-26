using System.Collections.Generic;

namespace Sfa.Das.Sas.Shared.Components.ViewModels
{
    public class SearchQueryViewModel
    {
        public string Keywords { get; set; }
        public int ResultsToTake { get; set; } = 20;
        public int Page { get; set; } = 1;
        public int SortOrder { get; set; } = 0;

        public List<int> SelectedLevels => new List<int>() {0, 1, 2, 3, 4, 5, 6, 7, 8};
    }
}