namespace Sfa.Eds.Das.Web.Services.Factories
{
    using System;
    using Models;
    using Nest;

    public interface IElasticsearchClientFactory
    {
        ElasticClient Create();
    }

    public class ElasticsearchClientFactory : IElasticsearchClientFactory
    {
        private readonly IApplicationSettings applicationSettings;

        public ElasticsearchClientFactory(IApplicationSettings applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public ElasticClient Create()
        {
            var node = new Uri(this.applicationSettings.SearchHost);

            var settings = new ConnectionSettings(
                node,
                defaultIndex: "cistandardindexesalias");

            settings.MapDefaultTypeNames(d => d.Add(typeof(SearchResultsItem), "standarddocument"));

            return new ElasticClient(settings);
        }
    }
}