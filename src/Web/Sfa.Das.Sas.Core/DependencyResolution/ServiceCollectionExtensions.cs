using Microsoft.Extensions.DependencyInjection;

namespace Sfa.Das.Sas.Core.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFavouritesBasket(this IServiceCollection services, string redisConnectionString, int expiryLengthInDays = 90)
        {
            services.AddScoped<IRetryWebRequests, WebRequestRetryService>();
        }
    }
}