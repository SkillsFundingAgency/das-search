namespace Sfa.Eds.Das.Core.BusinessLogic.Services.Factories
{
    using System;

    using Nest;

    using Interfaces;
    using Models;

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IApplicationSettings applicationSettings;

        public ElasticsearchClientFactory(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public IElasticClient Create()
        {
            var node = new Uri(this.applicationSettings.SearchHost);

            var settings = new ConnectionSettings(node, defaultIndex: this.applicationSettings.StandardIndexesAlias);

            settings.MapDefaultTypeNames(d => d.Add(typeof(StandardSearchResultsItem), "standarddocument"));
            
            return new ElasticClient(settings);
        }
    }
}