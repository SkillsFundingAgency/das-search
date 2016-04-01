using Sfa.Das.ApplicationServices;
using Sfa.Eds.Das.Core.Configuration;
using Sfa.Eds.Das.Core.Domain.Services;
using Sfa.Eds.Das.Core.Logging;
using Sfa.Eds.Das.Infrastructure.Configuration;
using Sfa.Eds.Das.Infrastructure.ElasticSearch;
using Sfa.Eds.Das.Infrastructure.Logging;
using Sfa.Eds.Das.Infrastructure.Mapping;
using Sfa.Eds.Das.Infrastructure.PostCodeIo;
using StructureMap.Configuration.DSL;

namespace Sfa.Eds.Das.Infrastructure.DependencyResolution
{
    using Sfa.Eds.Das.Infrastructure.Elasticsearch;

    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType)).AlwaysUnique();
            For<IConfigurationSettings>().Use<ApplicationSettings>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<ILookupLocations>().Use<PostCodesIOLocator>();
            For<IGetStandards>().Use<StandardRepository>();
            For<IGetFrameworks>().Use<FrameworkRepository>();
            For<ISearchProvider>().Use<ElasticsearchProvider>();
            For<IRetryWebRequests>().Use<WebRequestRetryService>();
            For<IApprenticeshipProviderRepository>().Use<ApprenticeshipProviderRepository>();
            For<IStandardMapping>().Use<StandardMapping>();
            For<IFrameworkMapping>().Use<FrameworkMapping>();
            For<IProviderMapping>().Use<ProviderMapping>();
        }
    }
}
