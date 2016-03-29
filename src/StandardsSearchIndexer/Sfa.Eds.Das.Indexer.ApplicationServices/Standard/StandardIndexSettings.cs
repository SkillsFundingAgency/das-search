namespace Sfa.Eds.Das.Indexer.ApplicationServices.Standard
{
    using System.Configuration;

    using Services;
    using Sfa.Eds.Das.Indexer.ApplicationServices.Settings;

    public class StandardIndexSettings : IIndexSettings<IMaintainApprenticeshipIndex>
    {
        public string IndexesAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string StandardProviderDocumentType => ConfigurationManager.AppSettings["StandardProviderDocumentType"];

        public string FrameworkProviderDocumentType => ConfigurationManager.AppSettings["FrameworkProviderDocumentType"];
    }
}