using System.Configuration;
using Sfa.Das.Sas.Indexer.ApplicationServices.Services;
using Sfa.Das.Sas.Indexer.ApplicationServices.Settings;

namespace Sfa.Das.Sas.Indexer.ApplicationServices.Standard
{
    public class StandardIndexSettings : IIndexSettings<IMaintainApprenticeshipIndex>
    {
        public string IndexesAlias => ConfigurationManager.AppSettings["ApprenticeshipIndexAlias"];

        public string PauseTime => ConfigurationManager.AppSettings["PauseTime"];

        public string StandardProviderDocumentType => ConfigurationManager.AppSettings["StandardProviderDocumentType"];

        public string FrameworkProviderDocumentType => ConfigurationManager.AppSettings["FrameworkProviderDocumentType"];
    }
}