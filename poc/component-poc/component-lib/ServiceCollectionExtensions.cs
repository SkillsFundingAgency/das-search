using Microsoft.Extensions.DependencyInjection;
using shared_lib;

namespace component_lib
{
    public static class ServiceCollectionExtensions
    {
         public static void AddFatComponents(this IServiceCollection services)
         {
            services.AddScoped<IGenerateSearchResults, SearchResultsGenerator>();             
         }
    }
}