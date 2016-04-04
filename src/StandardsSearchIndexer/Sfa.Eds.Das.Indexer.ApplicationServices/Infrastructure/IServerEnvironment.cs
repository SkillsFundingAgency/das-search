namespace Sfa.Eds.Das.Indexer.ApplicationServices.Infrastructure
{
    public interface IServerEnvironment
    {
        string GetEnvironmentVariable(string settingName);
    }
}
