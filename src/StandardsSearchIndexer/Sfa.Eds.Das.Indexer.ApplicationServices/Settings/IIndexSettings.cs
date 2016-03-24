namespace Sfa.Eds.Das.Indexer.ApplicationServices.Settings
{
    public interface IIndexSettings<T>
    {
        string SearchHost { get; }
        string IndexesAlias { get; }
        string PauseTime { get; }

        string StandardProviderDocumentType { get; }

        string FrameworkProviderDocumentType { get; }
    }
}