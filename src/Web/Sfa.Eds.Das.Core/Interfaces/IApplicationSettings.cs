namespace Sfa.Eds.Das.Core.Interfaces
{
    public interface IApplicationSettings
    {
        string SearchHost { get; }
        string StandardIndexesAlias { get; }
        string ProviderIndexAlias { get; }
        string ElasticServerIp { get; }
    }
}