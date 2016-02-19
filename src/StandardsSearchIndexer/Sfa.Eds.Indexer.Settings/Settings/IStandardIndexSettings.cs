namespace Sfa.Eds.Das.Indexer.Settings.Settings
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
        string QueueName { get; }
        string PauseTime { get; }
        string StandardJsonContainer { get; }
        string StandardPdfContainer { get; }
        string StandardContentType { get; }
    }
}