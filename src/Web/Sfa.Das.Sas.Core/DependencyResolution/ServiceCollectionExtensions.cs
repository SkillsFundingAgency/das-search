using Microsoft.Extensions.DependencyInjection;

namespace Sfa.Das.Sas.Core.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddScoped<IRetryWebRequests, WebRequestRetryService>();
        }
    }
}