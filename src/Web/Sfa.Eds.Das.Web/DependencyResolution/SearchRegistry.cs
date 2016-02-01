namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using Elasticsearch.Net;

    using Services;

    using Sfa.Eds.Das.Web.Services.Factories;

    using StructureMap.Configuration.DSL;

    public class SearchRegistry : Registry
    {
        public SearchRegistry()
        {
            For<ISearchForStandards>().Use<SearchService>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IApplicationSettings>().Use<ApplicationSettings>();
        }
    }
}