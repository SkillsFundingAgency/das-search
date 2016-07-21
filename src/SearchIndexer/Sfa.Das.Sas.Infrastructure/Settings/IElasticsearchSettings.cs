namespace Sfa.Das.Sas.Indexer.Infrastructure.Settings
{
    public interface IElasticsearchSettings
    {
        string[] StopWords { get; }

        string[] StopWordsExtended { get; }

        string[] Synonyms { get; }

        string ApprenticeshipIndexShards { get; }

        string ApprenticeshipIndexReplicas { get; }

        string ProviderIndexShards { get; }

        string ProviderIndexReplicas { get; }
    }
}