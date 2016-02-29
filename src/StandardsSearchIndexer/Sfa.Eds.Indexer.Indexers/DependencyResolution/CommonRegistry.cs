using Sfa.Eds.Das.Indexer.Common.AzureAbstractions;
using Sfa.Eds.Das.Indexer.Common.Helpers;
using Sfa.Eds.Das.Indexer.Common.Services;
using Sfa.Eds.Das.Indexer.Common.Settings;
using StructureMap;

namespace Sfa.Eds.Das.Indexer.Common.DependencyResolution
{
    using Sfa.Eds.Das.Indexer.Common.Models;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<IGenericControlQueueConsumer>().Use<GenericControlQueueConsumer>();
            For<ICloudBlobContainerWrapper>().Use<CloudBlobContainerWrapper>();
            For<ICloudBlobClientWrapper>().Use<CloudBlobClientWrapper>();
            For<IAzureSettings>().Use<AzureSettings>();
            For<ICommonSettings>().Use<CommonSettings>();
            For<ICloudQueueService>().Use<CloudQueueService>();
            For<IBlobStorageHelper>().Use<BlobStorageHelper>();
            For<IIndexerService<Provider>>().Use<IndexerService<Provider>>();
            For<IIndexerService<StandardDocument>>().Use<IndexerService<StandardDocument>>();
        }
    }
}