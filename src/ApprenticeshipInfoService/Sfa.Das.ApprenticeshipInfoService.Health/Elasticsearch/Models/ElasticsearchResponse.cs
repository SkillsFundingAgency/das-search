namespace Sfa.Das.ApprenticeshipInfoService.Health.Elasticsearch.Models
{
    using System;
    using System.Collections.Generic;

    public class ElasticsearchResponse
    {
        public List<ElasticsearchAlias> ElasticsearchAliases { get; set; }

        public Exception Exception { get; set; }
    }
}