namespace Sfa.Eds.Das.Core.BusinessLogic.Services
{
    using System.Configuration;

    using Sfa.Eds.Das.Core.Interfaces;

    public class ApplicationSettings : IApplicationSettings
    {
        public string SearchHost => ConfigurationManager.AppSettings["SearchHost"];

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string ElasticServerIp => ConfigurationManager.AppSettings["ElasticServerIp"];
    }
}