using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sfa.Das.Sas.Web.Settings;

namespace Sfa.Das.Sas.Web.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GeneralSettings>(configuration.GetSection("General"));

            // TODO: LWA Uncomment when services have been moved in.
            // services.AddScoped<ServiceFactory>();
            // services.AddScoped<IMappingService, MappingService>();
            // services.AddScoped<ICookieService, CookieService>();
            // services.AddScoped<IGetProviderDetails, ProviderApiRepository>();
            // services.AddScoped<IUrlEncoder, UrlEncoder>();
            // services.AddScoped<IXmlDocumentSerialiser, XmlDocumentSerialiser>();
            // services.AddScoped<IHttpCookieFactory, HttpCookieFactory>();
            // services.AddScoped<IValidation, Validation>();
            // services.AddScoped<IButtonTextService, ButtonTextService>();

            // TODO: LWA Need to determine if this is needed in the web???
            //services.AddScoped<IProviderApiClient>();

            // TODO: LWA - Don't think we'll use this
            //For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));
        }
    }
}