namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    public interface IElasticsearchSettings
    {
        string[] StopWords { get; }

        string[] StopWordsExtended { get; }

        string[] Synonyms { get; }
    }
}