using System.Configuration;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Provider
{
    public class ProviderIndexSettings : IIndexSettings<IMaintainProviderIndex>, IProviderFeatures
    {
        public string IndexesAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public bool FilterInactiveProviders => bool.Parse(ConfigurationManager.AppSettings["Feature.FilterInactiveProviders"] ?? "false");

        public string StandardProviderDocumentType => ConfigurationManager.AppSettings["StandardProviderDocumentType"];

        public string FrameworkProviderDocumentType => ConfigurationManager.AppSettings["FrameworkProviderDocumentType"];
    }
}