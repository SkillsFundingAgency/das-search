using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.Interfaces;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Elasticsearch;
using Sfa.Das.Sas.Infrastructure.Mapping;
using Sfa.Das.Sas.Infrastructure.PostCodeIo;
using Sfa.Das.Sas.Infrastructure.Repositories;
using Sfa.Das.Sas.Infrastructure.Settings;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.AssessmentOrgs.Api.Client;

namespace Sfa.Das.Sas.Shared.Components.DependencyResolution
{
    public static class SharedComponentsRegistry
    {
        public static void AddFatSharedComponents(this IServiceCollection services, bool useElastic = false)
        {
            //   services.AddTransient<IMyService, MyService>();

            services.AddMediatR(typeof(ApprenticeshipSearchQuery));


            //Application DI
            AddApplicationServices(services);

            //Infrastructure DI
            AddInfrastructureServices(services);

            if (useElastic)
            {
                AddElasticSearchServices(services);
            }


        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddTransient<IHttpGet, HttpService>();
            services.AddTransient<IApprenticeshipSearchService, ApprenticeshipSearchService>();
            services.AddTransient<IProviderSearchService, ProviderSearchService>();
            services.AddTransient<IPaginationSettings, PaginationSettings>();
            services.AddTransient<AbstractValidator<ProviderSearchQuery>, ProviderSearchQueryValidator>();
            services.AddTransient<AbstractValidator<ApprenticeshipProviderDetailQuery>, ApprenticeshipProviderDetailQueryValidator>();
            services.AddTransient<AbstractValidator<GetFrameworkQuery>, FrameworkQueryValidator>();
            services.AddTransient<IPostcodeIoService, PostcodeIoService>();
        }

        private static void AddInfrastructureServices(IServiceCollection services)
        {
            services.AddTransient<IConfigurationSettings, ApplicationSettings>();

            services.AddTransient<ILookupLocations, PostCodesIoLocator>();

            services.AddTransient<IGetFrameworks, FrameworkApiRepository>();
            services.AddTransient<IGetStandards, StandardApiRepository>();
            services.AddTransient<IApprenticeshipProviderRepository, ApprenticeshipProviderApiRepository>();

            services.AddTransient<IProviderNameSearchProvider,ProviderNameSearchProvider>();

            services.AddTransient<IStandardApiClient, StandardApiClient>(service => new StandardApiClient(new ApplicationSettings().ApprenticeshipApiBaseUrl));
            services.AddTransient<IFrameworkApiClient, FrameworkApiClient>(service => new FrameworkApiClient(new ApplicationSettings().ApprenticeshipApiBaseUrl));
            services.AddTransient<IAssessmentOrgsApiClient, AssessmentOrgsApiClient>(service => new AssessmentOrgsApiClient(new ApplicationSettings().ApprenticeshipApiBaseUrl));

            services.AddTransient<IStandardMapping, StandardMapping>();
            services.AddTransient<IFrameworkMapping, FrameworkMapping>();
            services.AddTransient<IProviderMapping, ProviderMapping>();
            services.AddTransient<IProviderNameSearchMapping,ProviderNameSearchMapping>();

            services.AddTransient<IPaginationOrientationService,PaginationOrientationService>();
        }

        private static void AddElasticSearchServices(IServiceCollection services)
        {
            services.AddTransient<IElasticsearchClientFactory,ElasticsearchClientFactory>();
            services.AddTransient<IElasticsearchCustomClient, ElasticsearchCustomClient>();
            services.AddTransient<IElasticsearchHelper,ElasticsearchHelper>();


            services.AddTransient<IApprenticeshipSearchProvider,ElasticsearchApprenticeshipSearchProvider>();
            services.AddTransient<IProviderLocationSearchProvider,ElasticsearchProviderLocationSearchProvider>();

            services.AddTransient<IProviderNameSearchProviderQuery,ProviderNameSearchProviderQuery>();

            services.AddTransient<IGetProviders,ProviderElasticRepository>();
        }
    }
}
