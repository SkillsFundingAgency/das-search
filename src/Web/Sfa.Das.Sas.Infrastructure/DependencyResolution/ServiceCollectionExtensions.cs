using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Sfa.Das.FatApi.Client.Api;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Core.Domain.Repositories;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Infrastructure.Mapping;
using Sfa.Das.Sas.Infrastructure.PostCodeIo;
using Sfa.Das.Sas.Infrastructure.Providers;
using Sfa.Das.Sas.Infrastructure.Repositories;
using Sfa.Das.Sas.Infrastructure.Settings;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.AssessmentOrgs.Api.Client;
using SFA.DAS.Providers.Api.Client;

namespace Sfa.Das.Sas.Infrastructure.DependencyResolution
{
    public static class ServicesCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FatSettings>(configuration.GetSection("FatApi"));
            services.Configure<PostcodeLookupSettings>(configuration.GetSection("PostcodeLookup"));

            services.AddScoped<ILookupLocations, PostCodesIoLocator>();

            services.AddScoped<IApprenticeshipSearchProvider, ApprenticeshipsSearchApiProvider>();
            services.AddScoped<IGetFrameworks, FrameworkApiRepository>();
            services.AddScoped<IGetStandards, StandardApiRepository>();
            services.AddScoped<IGetAssessmentOrganisations, AssessmentOrganisationApiRepository>();
            services.AddScoped<IApprenticeshipProviderRepository, ApprenticeshipProviderApiRepository>();
           
            services.AddScoped<IStandardMapping, StandardMapping>();
            services.AddScoped<IFrameworkMapping, FrameworkMapping>();
            services.AddScoped<IProviderMapping, ProviderMapping>();
            services.AddScoped<IAssessmentOrganisationMapping, AssessmentOrganisationMapping>();
            services.AddScoped<IProviderNameSearchMapping, ProviderNameSearchMapping>();
            services.AddScoped<IApprenticeshipSearchResultsMapping, ApprenticeshipSearchResultsMapping>();
            services.AddScoped<IApprenticeshipSearchResultsItemMapping, ApprenticeshipSearchResultsItemMapping>();
            services.AddScoped<ISearchResultsMapping, SearchResultsMapping>();
            services.AddScoped<IProviderSearchResultsMapper, ProviderSearchResultsMapper>();
            services.AddScoped<IPaginationOrientationService, PaginationOrientationService>();

            var fatApiBaseUrl = configuration.GetValue<string>("FatApi:ApiBaseUrl");

            services.AddScoped<IStandardApiClient, StandardApiClient>(c => new StandardApiClient(fatApiBaseUrl));
            services.AddScoped<IFrameworkApiClient, FrameworkApiClient>(c => new FrameworkApiClient(fatApiBaseUrl));
            services.AddScoped<IAssessmentOrgsApiClient, AssessmentOrgsApiClient>(c => new AssessmentOrgsApiClient(fatApiBaseUrl));

            services.AddRefitClient<ISearchApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(fatApiBaseUrl));

           services.AddRefitClient<ISearchVApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(fatApiBaseUrl));

           services.AddRefitClient<IProvidersVApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(fatApiBaseUrl));

            services.AddScoped<IProviderApiClient>(c => new ProviderApiClient(fatApiBaseUrl));
            
            services.AddScoped<IProviderSearchProvider, ProviderApiRepository>();

            services.AddScoped<IPaginationOrientationService, PaginationOrientationService>();
        }
    }
}