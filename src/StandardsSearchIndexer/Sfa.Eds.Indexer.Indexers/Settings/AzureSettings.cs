using System;
using System.Configuration;

namespace Sfa.Eds.Das.Indexer.Common.Settings
{
    public class AzureSettings : IAzureSettings
    {
        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => $"DefaultEndpointsProtocol=https;AccountName={StorageAccountName};AccountKey={StorageAccountKey}";

        public string QueueName(Type type)
        {
            var name = type.Name.Replace("IndexerService", string.Empty).Substring(1) + ".QueueName";
            var setting = ConfigurationManager.AppSettings[name];
            if (setting != null)
            {
                return setting;
            }

            throw new ArgumentException("setting '" + name + "' not found");
        }
    }
}