using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Configuration;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;
using Sfa.Das.Sas.Infrastructure.PostCodeIo;
using StructureMap.Configuration.DSL;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
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
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
        }
    }
}
