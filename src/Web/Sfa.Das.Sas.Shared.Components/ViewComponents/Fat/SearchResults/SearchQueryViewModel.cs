namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class SearchQueryViewModel
    {
        public string Keywords { get; set; }
        public int ResultsToTake { get; set; } = 20;
        public int Page { get; set; } = 1;
        public int SortOrder { get; set; } = 0;
    }
}