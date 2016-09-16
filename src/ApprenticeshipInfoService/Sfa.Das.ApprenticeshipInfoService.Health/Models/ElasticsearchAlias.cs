namespace Sfa.Das.ApprenticeshipInfoService.Health.Models
{
    public class ElasticsearchAlias
    {
        public string AliasName { get; set; }

        public string Status { get; set; }

        public string IndexName { get; set; }

        public string DocumentCount { get; set; }

        public string Health { get; set; }
    }
}