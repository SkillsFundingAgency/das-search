using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Eds.Standards.Indexer.AzureWorkerRole.Settings
{
    public interface IStandardIndexSettings
    {
        string SearchHost { get; }
        string StandardIndexesAlias { get; }
        string SearchEndpointConfigurationName { get; }
        string DatasetName { get; }
        string StandardDescriptorName { get; }
        string StorageAccountName { get; }
        string StorageAccountKey { get; }
        string ConnectionString { get; }
    }
}
