namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using log4net;

    using Sfa.Das.ApplicationServices;
    using Sfa.Eds.Das.Core.Configuration;
    using Sfa.Eds.Das.Core.Domain.Services;
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
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IConfigurationSettings>().Use<ApplicationSettings>();
            For<IMappingService>().Use<MappingService>();
            For<log4net.ILog>().Use(LogManager.GetLogger(Log4NetSettings.LoggerName));
            For<Core.Logging.ILog>().Use<ApplicationLogger>();

            // New infrastructure
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();

            For<IStandardSearchService>().Use<StandardSearchService>();
            For<IStandardRepository>().Use<StandardRepository>();
            For<ISearchProvider>().Use<ElasticsearchProvider>();
        }
    }
}