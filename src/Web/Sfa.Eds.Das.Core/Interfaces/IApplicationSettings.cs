namespace Sfa.Eds.Das.Core.Interfaces
{
    public interface IApplicationSettings
    {
        string SearchHost { get; }
        string StandardIndexesAlias { get; }
        string ElasticServerIp { get; }
    }
}