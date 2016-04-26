namespace Sfa.Das.Sas.Indexer.ApplicationServices.Infrastructure
{
    public interface IServerEnvironment
    {
        string GetEnvironmentVariable(string settingName);
    }
}
