namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using log4net;

    using Sfa.Das.ApplicationServices;
    using Sfa.Eds.Das.Core.Configuration;
    using Sfa.Eds.Das.Core.Interfaces.Search;
    using Sfa.Eds.Das.Core.Logging;
    using Sfa.Eds.Das.Infrastructure.Configuration;
    using Sfa.Eds.Das.Infrastructure.ElasticSearch;
    using Sfa.Eds.Das.Infrastructure.Logging;
    using Sfa.Eds.Das.Web.Services;

    using StructureMap.Configuration.DSL;

    public class SearchRegistry : Registry
    {
        public SearchRegistry()
        {
            //For<ISearchService>().Use<SearchService>();
            //this.For<IProviderSearchProvider>().Use<ElasticSearchProviderSearchProvider>();
            this.For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            this.For<IApplicationSettings>().Use<ApplicationSettings>();
            this.For<IMappingService>().Use<MappingService>();
            this.For<ILog>().Use(LogManager.GetLogger(Log4NetSettings.LoggerName));
            this.For<IApplicationLogger>().Use<ApplicationLogger>();

            // New infrastructure

            this.For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();

            For<IStandardSearchService>().Use<StandardSearchService>();
            For<IStandardRepository>().Use<StandardRepository>();
            For<ISearchProvider>().Use<ElasticsearchProvider>();

        }
    }
}