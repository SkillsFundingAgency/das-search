using System;

namespace Sfa.Eds.Das.Web.Models
{
    public sealed class SearchResultsItem
    {
        public string Title { get; set; }
        public float FundingCap { get; set; }
        public DateTime EffectiveEndDay { get; set; }
    }
}