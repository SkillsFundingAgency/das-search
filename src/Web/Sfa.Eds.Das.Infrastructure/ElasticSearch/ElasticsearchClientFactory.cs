namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System;

    using Nest;

    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Configuration;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IConfigurationSettings applicationSettings;

        public ElasticsearchClientFactory(IConfigurationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public IElasticClient Create()
        {
            var node = new Uri(this.applicationSettings.SearchHost);

            var settings = new ConnectionSettings(node, defaultIndex: this.applicationSettings.StandardIndexesAlias);

            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardSearchResultsItem), "standarddocument"));
            settings.MapDefaultTypeNames(d => d.Add(typeof(ProviderSearchResultsItem), "provider"));

            return new ElasticClient(settings);
        }
    }
}