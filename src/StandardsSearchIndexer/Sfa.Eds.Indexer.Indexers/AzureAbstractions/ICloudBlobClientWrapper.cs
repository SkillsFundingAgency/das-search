namespace Sfa.Eds.Das.Indexer.Common.AzureAbstractions
{
    using Sfa.Eds.Das.Indexer.Common.Helpers;

    public interface ICloudBlobClientWrapper
    {
        ICloudBlobContainerWrapper GetContainerReference(string containerName);
    }
}