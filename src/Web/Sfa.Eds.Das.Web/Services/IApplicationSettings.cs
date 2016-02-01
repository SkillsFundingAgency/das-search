namespace Sfa.Eds.Das.Web.Services
{
    public interface IApplicationSettings
    {
        string SearchHost { get; }
        string StandardIndexesAlias { get; }
        string ElasticServerIp { get; }
    }
}