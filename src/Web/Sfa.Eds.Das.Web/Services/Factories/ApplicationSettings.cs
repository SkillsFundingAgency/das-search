namespace Sfa.Eds.Das.Web.Services.Factories
{
    using System.Configuration;

    public class ApplicationSettings : IApplicationSettings
    {
        public string SearchHost => ConfigurationManager.AppSettings["SearchHost"];
    }
}