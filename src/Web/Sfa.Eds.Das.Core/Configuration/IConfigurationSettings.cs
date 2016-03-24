namespace Sfa.Eds.Das.Core.Configuration
{
    public interface IConfigurationSettings
    {
        string SearchHost { get; }

        string ApprenticeshipIndexAlias { get; }

        string ProviderIndexAlias { get; }

        string ElasticServerIp { get; }

        string BuildId { get; }
    }
}