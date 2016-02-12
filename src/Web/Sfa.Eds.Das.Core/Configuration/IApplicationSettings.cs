namespace Sfa.Eds.Das.Core.Configuration
{
    public interface IApplicationSettings
    {
        string SearchHost { get; }
        string StandardIndexesAlias { get; }
        string ProviderIndexAlias { get; }
        string ElasticServerIp { get; }
    }
}