namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using global::Elasticsearch.Net.ConnectionPool;

    using Nest;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Configuration;

    public sealed class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchClientFactory(IConfigurationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public IElasticClient Create()
        {
            var pool = new SniffingConnectionPool(_applicationSettings.ElasticServerUrls);
            var settings = new ConnectionSettings(pool);

            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardSearchResultsItem), "standarddocument"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardProviderSearchResultsItem), "standardprovider"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(FrameworkProviderSearchResultsItem), "frameworkprovider"));

            return new ElasticClient(settings);
        }
    }
}