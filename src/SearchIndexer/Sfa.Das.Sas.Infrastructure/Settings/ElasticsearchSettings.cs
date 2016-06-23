namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    using System.Configuration;

    public class ElasticsearchSettings : IElasticsearchSettings
    {
        public string[] StopWords => GetSetting("StopWords");

        public string[] StopWordsExtended => GetSetting("StopWordsExtended");

        public string[] Synonyms => GetSetting("Synonyms");

        private string[] GetSetting(string configName)
        {
            var str = ConfigurationManager.AppSettings[configName];
            return !string.IsNullOrEmpty(str) ? str.Split('|') : new[] { string.Empty };
        }
    }
}
