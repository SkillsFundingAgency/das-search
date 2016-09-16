namespace Sfa.Das.ApprenticeshipInfoService.Health.DependencyResolution
{
    using Sfa.Das.ApprenticeshipInfoService.Health.Elasticsearch;
    using Sfa.Das.ApprenticeshipInfoService.Health.Elasticsearch.Models;

    using StructureMap;

    public class HealthRegistry : Registry
    {
        public HealthRegistry()
        {
            For<IHealthSettings>().Use<HealthSettings>();
            For<IElasticsearchService>().Use<ElasticsearchService>();
            For<IHttpServer>().Use<HttpService>();
            For<IHealthService>().Use<HealthService>();
        }
    }
}
