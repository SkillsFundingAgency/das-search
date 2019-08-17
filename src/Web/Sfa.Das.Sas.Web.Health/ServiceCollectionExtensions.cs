using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sfa.Das.Sas.Web.Health
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHealth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HealthSettings>(configuration.GetSection("Health"));

            services.AddScoped<IHealthService, HealthService>();
        }
    }
}