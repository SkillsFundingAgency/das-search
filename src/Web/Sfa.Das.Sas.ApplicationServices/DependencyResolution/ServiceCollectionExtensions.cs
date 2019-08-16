using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Das.Sas.ApplicationServices.Http;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.ApplicationServices.Services;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.ApplicationServices.Validators;

namespace Sfa.Das.Sas.ApplicationServices.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFavouritesBasket(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);

            services.AddScoped<IHttpGet, HttpService>();
            services.AddScoped<IApprenticeshipSearchService, ApprenticeshipSearchService>();
            services.AddScoped<IProviderSearchService, ProviderSearchService>();
            services.AddScoped<AbstractValidator<ProviderSearchQuery>, ProviderSearchQueryValidator>();
            services.AddScoped<AbstractValidator<ApprenticeshipProviderDetailQuery>, ApprenticeshipProviderDetailQueryValidator>();
            services.AddScoped<AbstractValidator<GetFrameworkQuery>, FrameworkQueryValidator>();
            services.AddScoped<IPostcodeIoService, PostcodeIoService>();

            services.Configure<PaginationSettings>(configuration.GetSection("PaginationSettings"));
        }
    }
}