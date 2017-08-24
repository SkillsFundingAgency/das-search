using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Infrastructure.Repositories;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
    using FeatureToggle.Core.Fluent;
    using SFA.DAS.Apprenticeships.Api.Client;
    using Sfa.Das.Sas.ApplicationServices;
    using Sfa.Das.Sas.ApplicationServices.FeatureToggles;
    using Sfa.Das.Sas.ApplicationServices.Settings;
    using Sfa.Das.Sas.Core.Configuration;
    using Sfa.Das.Sas.Core.Domain.Services;
    using Sfa.Das.Sas.Infrastructure.Elasticsearch;
    using Sfa.Das.Sas.Infrastructure.Mapping;
    using Sfa.Das.Sas.Infrastructure.PostCodeIo;
    using Sfa.Das.Sas.Infrastructure.Settings;
    using StructureMap;

    public sealed class InfrastructureRegistry : Registry
    {
        public InfrastructureRegistry()
        {
            For<SFA.DAS.NLog.Logger.ILog>().Use(x => new SFA.DAS.NLog.Logger.NLogLogger(
                x.ParentType,
                x.GetInstance<SFA.DAS.NLog.Logger.IRequestContext>(),
                GetProperties()
                )).AlwaysUnique();
            For<IConfigurationSettings>().Use<ApplicationSettings>();
            For<ICookieSettings>().Use<CookieSettings>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<IElasticsearchClientFactory>().Use<ElasticsearchClientFactory>();
            For<ILookupLocations>().Use<PostCodesIoLocator>();

            For<IGetProviders>().Use<ProviderElasticRepository>();

            For<IElasticsearchHelper>().Use<ElasticsearchHelper>();

            if (Is<ApprenticeshipServiceApiFeature>.Enabled)
            {
                For<IGetFrameworks>().Use<FrameworkApiRepository>();
                For<IGetStandards>().Use<StandardApiRepository>();
                For<IApprenticeshipProviderRepository>().Use<ApprenticeshipProviderApiRepository>();
            }
            else
            {
                For<IGetFrameworks>().Use<FrameworkElasticRepository>();
                For<IGetStandards>().Use<StandardElasticRepository>();
                For<IApprenticeshipProviderRepository>().Use<ApprenticeshipProviderRepository>();
            }

            For<IStandardApiClient>().Use<StandardApiClient>().Ctor<string>("baseUri").Is(new ApplicationSettings().ApprenticeshipApiBaseUrl);
            For<IFrameworkApiClient>().Use<FrameworkApiClient>().Ctor<string>("baseUri").Is(new ApplicationSettings().ApprenticeshipApiBaseUrl);
            For<IApprenticeshipSearchProvider>().Use<ElasticsearchApprenticeshipSearchProvider>();
            For<IProviderLocationSearchProvider>().Use<ElasticsearchProviderLocationSearchProvider>();
            For<IStandardMapping>().Use<StandardMapping>();
            For<IFrameworkMapping>().Use<FrameworkMapping>();
            For<IProviderMapping>().Use<ProviderMapping>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
        }

        private IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object>();
            properties.Add("Version", GetVersion());
            return properties;
        }

        private string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }
    }
}
