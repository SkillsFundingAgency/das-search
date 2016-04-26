namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    public interface ILarsSettings
    {
        string SearchEndpointConfigurationName { get; }
        string DatasetName { get; }
        string StandardDescriptorName { get; }
    }
}