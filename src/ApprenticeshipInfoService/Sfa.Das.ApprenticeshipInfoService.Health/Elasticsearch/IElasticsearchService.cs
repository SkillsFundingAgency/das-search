namespace Sfa.Das.ApprenticeshipInfoService.Health.Elasticsearch
{
    using Models;
    using System;
    using System.Collections.Generic;

    using Health.Models;

    public interface IElasticsearchHealthService
    {
        ElasticsearchLog GetErrorLogs(IEnumerable<Uri> uriStrings, string environment);

        ElasticsearchResponse GetElasticHealth(IEnumerable<Uri> uris, string environment);
    }
}