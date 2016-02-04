namespace Sfa.Eds.Das.Web.DependencyResolution
{
    using log4net;

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
            For<ILog>().Use(LogManager.GetLogger(Log4NetSettings.LoggerName));
        }
    }
}