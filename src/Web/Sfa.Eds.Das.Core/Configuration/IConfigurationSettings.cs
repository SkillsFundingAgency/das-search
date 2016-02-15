namespace Sfa.Eds.Das.Core.Configuration
{
    public interface IConfigurationSettings
    {
        string SearchHost { get; }

        string StandardIndexesAlias { get; }

        string ProviderIndexAlias { get; }

        string ElasticServerIp { get; }

        string BuildId { get; }
    }
}