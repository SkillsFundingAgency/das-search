using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using ApplicationServices;
    using ApplicationServices.FeatureToggles;
    using ApplicationServices.Interfaces;
    using ApplicationServices.Settings;
    using Core.Configuration;
    using Core.Domain.Repositories;
    using Core.Domain.Services;
    using Elasticsearch;
    using FeatureToggle.Core.Fluent;
    using Mapping;
    using PostCodeIo;
    using Repositories;
    using Settings;
    using SFA.DAS.Apprenticeships.Api.Client;
    using SFA.DAS.AssessmentOrgs.Api.Client;
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

            For<IGetFrameworks>().Use<FrameworkApiRepository>();
            For<IGetStandards>().Use<StandardApiRepository>();
            For<IApprenticeshipProviderRepository>().Use<ApprenticeshipProviderApiRepository>();
    
            For<IStandardApiClient>().Use<StandardApiClient>().Ctor<string>("baseUri").Is(new ApplicationSettings().ApprenticeshipApiBaseUrl);
            For<IFrameworkApiClient>().Use<FrameworkApiClient>().Ctor<string>("baseUri").Is(new ApplicationSettings().ApprenticeshipApiBaseUrl);
            For<IAssessmentOrgsApiClient>().Use<AssessmentOrgsApiClient>().Ctor<string>("baseUri").Is(new ApplicationSettings().ApprenticeshipApiBaseUrl);
            For<IApprenticeshipSearchProvider>().Use<ElasticsearchApprenticeshipSearchProvider>();
            For<IProviderLocationSearchProvider>().Use<ElasticsearchProviderLocationSearchProvider>();
            For<IStandardMapping>().Use<StandardMapping>();
            For<IFrameworkMapping>().Use<FrameworkMapping>();
            For<IProviderMapping>().Use<ProviderMapping>();
            For<IProviderNameSearchMapping>().Use<ProviderNameSearchMapping>();
            For<IElasticsearchCustomClient>().Use<ElasticsearchCustomClient>();
            For<IProviderNameSearchProvider>().Use<ProviderNameSearchProvider>();
            For<IPaginationOrientationService>().Use<PaginationOrientationService>();
            For<IProviderNameSearchProviderQuery>().Use<ProviderNameSearchProviderQuery>();
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
