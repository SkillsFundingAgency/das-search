namespace Sfa.Das.Sas.Indexer.ApplicationServices.Settings
{
    public interface IIndexSettings<T>
    {
        string IndexesAlias { get; }

        string PauseTime { get; }

        string StandardProviderDocumentType { get; }

        string FrameworkProviderDocumentType { get; }
    }
}