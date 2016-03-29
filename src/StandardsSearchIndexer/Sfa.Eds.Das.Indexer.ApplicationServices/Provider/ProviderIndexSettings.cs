namespace Sfa.Eds.Das.Indexer.ApplicationServices.Provider
{
    using System.Configuration;
    using Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class ProviderIndexSettings : IIndexSettings<IMaintainProviderIndex>, IProviderFeatures
    {
        public string IndexesAlias => ConfigurationManager.AppSettings["ProviderIndexAlias"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public bool FilterInactiveProviders => bool.Parse(ConfigurationManager.AppSettings["Feature.FilterInactiveProviders"] ?? "false");

        public string StandardProviderDocumentType => ConfigurationManager.AppSettings["StandardProviderDocumentType"];

        public string FrameworkProviderDocumentType => ConfigurationManager.AppSettings["FrameworkProviderDocumentType"];
    }
}