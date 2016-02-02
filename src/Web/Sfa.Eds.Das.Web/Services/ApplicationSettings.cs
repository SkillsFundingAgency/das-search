namespace Sfa.Eds.Das.Web.Services
{
    using System.Configuration;

    public class ApplicationSettings : IApplicationSettings
    {
        public string SearchHost => ConfigurationManager.AppSettings["SearchHost"];

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];
    }
}