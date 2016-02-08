namespace Sfa.Eds.Das.Core.Models
{
    public sealed class SearchCriteria
    {
        public string Keywords { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}