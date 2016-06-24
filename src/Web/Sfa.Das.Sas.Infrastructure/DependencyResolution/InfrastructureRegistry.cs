using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Core.Logging;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Logging;
using Sfa.Das.Sas.Infrastructure.Mapping;
using Sfa.Das.Sas.Infrastructure.PostCodeIo;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
    using Sfa.Das.Sas.ApplicationServices.Settings;
    using Sfa.Das.Sas.Infrastructure.MiniProfiler;
    using Sfa.Das.Sas.Infrastructure.Settings;

    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(x.ParentType, x.GetInstance<IConfigurationSettings>())).AlwaysUnique();
            For<IConfigurationSettings>().Use<ApplicationSettings>();
            For<ICookieSettings>().Use<CookieSettings>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<ILookupLocations>().Use<PostCodesIoLocator>();
            For<IGetStandards>().Use<StandardRepository>();
            For<IGetFrameworks>().Use<FrameworkRepository>();
            For<IApprenticeshipSearchProvider>().Use<ElasticsearchApprenticeshipSearchProvider>();
            For<IProviderLocationSearchProvider>().Use<ElasticsearchProviderLocationSearchProvider>();
            For<IRetryWebRequests>().Use<WebRequestRetryService>();
            For<IApprenticeshipProviderRepository>().Use<ApprenticeshipProviderRepository>();
            For<IStandardMapping>().Use<StandardMapping>();
            For<IFrameworkMapping>().Use<FrameworkMapping>();
            For<IProviderMapping>().Use<ProviderMapping>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<IProfileAStep>().Use<MiniProfilerWrapper>();
        }
    }
}
