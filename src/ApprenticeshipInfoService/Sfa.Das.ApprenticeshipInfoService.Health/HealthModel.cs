namespace Sfa.Das.ApprenticeshipInfoService.Health
{
    using System.Collections.Generic;

    public class HealthModel
    {
        public string Status { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public IEnumerable<ElasticsearchAlias> ElasticSearchAliases { get; set; }

        public ElasticsearchLog ElasticsearchLog { get; set; }
    }

    public class ElasticsearchLog
    {
        public long  ErrorCount { get; set; }

        public IEnumerable<string> LogErrors { get; set; }
    }

    public class ElasticsearchAlias
    {
        public string AliasName { get; set; }

        public string Status { get; set; }

        public string IndexName { get; set; }

        public string DocumentCount { get; set; }

        public string Health { get; set; }
    }
    
    public static class Status
    {
        public static string Ok => "OK";

        public static string Error => "Error";
    }
}
