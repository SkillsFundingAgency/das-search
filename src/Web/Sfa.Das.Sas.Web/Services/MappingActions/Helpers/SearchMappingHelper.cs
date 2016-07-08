namespace Sfa.Das.Sas.Web.Services.MappingActions.Helpers
{
    public static class SearchMappingHelper
    {
        public static int CalculateLastPage(long totalResults, int resultsToTake)
        {
            return resultsToTake > 0 ? (int)System.Math.Ceiling((double)totalResults / resultsToTake) : 0;
        }
    }
}