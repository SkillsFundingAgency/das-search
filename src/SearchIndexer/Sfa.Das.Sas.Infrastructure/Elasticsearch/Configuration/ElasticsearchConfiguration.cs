namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration
{
    using Nest;

    using Settings;

    public class ElasticsearchConfiguration : IElasticsearchConfiguration
    {
        private readonly IElasticsearchSettings _elasticsearchSettings;

        public const string AnalyserEnglishCustom = "english_custom";
        public const string AnalyserEnglishCustomText = "english_custom_text";

        public ElasticsearchConfiguration(IElasticsearchSettings elasticsearchSettings)
        {
            _elasticsearchSettings = elasticsearchSettings;
        }

        public AnalysisDescriptor ApprenticeshipAnalysisDescriptor()
        {
            return new AnalysisDescriptor()
                        .TokenFilters(t => t
                            .Stemmer("english_possessive_stemmer", m => m.Language("possessive_english"))
                            .Stop("english_stop", m => m.StopWords(_elasticsearchSettings.StopWords))
                            .Stop("english_stop_freetext", m => m.StopWords(_elasticsearchSettings.StopWordsExtended))
                            .Stemmer("english_stemmer", m => m.Language("english"))
                            .Synonym("english_custom_synonyms", s => s.Synonyms(_elasticsearchSettings.Synonyms)))
                        .Analyzers(a => a
                            .Custom(AnalyserEnglishCustom, l => l
                                .Tokenizer("standard")
                                .Filters("english_possessive_stemmer", "lowercase", "english_stop", "english_custom_synonyms", "english_stemmer"))
                            .Custom(AnalyserEnglishCustomText, l => l
                                .Tokenizer("standard")
                                .Filters("english_possessive_stemmer", "lowercase", "english_stop_freetext", "english_custom_synonyms", "english_stemmer")));
        }
    }
}
