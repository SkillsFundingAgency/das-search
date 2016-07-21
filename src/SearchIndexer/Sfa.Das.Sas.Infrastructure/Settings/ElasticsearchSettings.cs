using System.Linq;

namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    using System.Configuration;

    public class ElasticsearchSettings : IElasticsearchSettings
    {
        public string[] StopWords => GetSetting("StopWords");

        public string[] StopWordsExtended => GetSetting("StopWordsExtended");

        public string[] Synonyms => GetSetting("Synonyms");

        public string ApprenticeshipIndexShards => GetSetting("ApprenticeshipIndexShards").FirstOrDefault();

        public string ApprenticeshipIndexReplicas => GetSetting("ApprenticeshipIndexReplicas").FirstOrDefault();

        public string ProviderIndexShards => GetSetting("ProviderIndexShards").FirstOrDefault();

        public string ProviderIndexReplicas => GetSetting("ProviderIndexReplicas").FirstOrDefault();

        private string[] GetSetting(string configName)
        {
            var str = ConfigurationManager.AppSettings[configName];
            return !string.IsNullOrEmpty(str) ? str.Split('|') : new[] { string.Empty };
        }
    }
}
