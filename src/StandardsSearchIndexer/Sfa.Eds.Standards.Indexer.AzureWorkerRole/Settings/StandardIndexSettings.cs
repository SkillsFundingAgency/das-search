using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings
{
    public class StandardIndexSettings : IStandardIndexSettings
    {
        public string SearchHost => ConfigurationManager.AppSettings["SearchHost"];

        public string StandardIndexesAlias => ConfigurationManager.AppSettings["StandardIndexesAlias"];

        public string SearchEndpointConfigurationName => ConfigurationManager.AppSettings["SearchEndpointConfigurationName"];

        public string DatasetName => ConfigurationManager.AppSettings["DatasetName"];

        public string StandardDescriptorName => ConfigurationManager.AppSettings["StandardDescriptorName"];

        public string StorageAccountName => ConfigurationManager.AppSettings["StorageAccountName"];

        public string StorageAccountKey => ConfigurationManager.AppSettings["StorageAccountKey"];

        public string ConnectionString => ConfigurationManager.AppSettings["ConnectionString"];
        public string QueueName => ConfigurationManager.AppSettings["QueueName"];
    }
}
