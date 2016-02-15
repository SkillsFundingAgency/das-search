namespace Sfa.Eds.Das.Web.Models
{
    public sealed class ProviderSearchCriteria
    {
        public string PostCode { get; set; }

        public string StandardId { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}