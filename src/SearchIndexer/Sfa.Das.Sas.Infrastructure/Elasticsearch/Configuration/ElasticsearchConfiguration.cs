namespace Sfa.Das.Sas.Indexer.Infrastructure.Elasticsearch.Configuration
{
    using Nest;

    using Settings;

    public class ElasticsearchConfiguration : IElasticsearchConfiguration
    {
        public const string AnalyserEnglishCustom = "english_custom";
        public const string AnalyserEnglishCustomText = "english_custom_text";
        private readonly IElasticsearchSettings _elasticsearchSettings;

        public ElasticsearchConfiguration(IElasticsearchSettings elasticsearchSettings)
        {
            _elasticsearchSettings = elasticsearchSettings;
        }

        public AnalysisDescriptor ApprenticeshipAnalysisDescriptor()
        {
            return new AnalysisDescriptor()
                        .CharFilters(t => t.PatternReplace("char_pattern_replace_er", m => m.Pattern("or\\b").Replacement("er")))
                        .TokenFilters(t => t
                            .Stemmer("english_possessive_stemmer", m => m.Language("possessive_english"))
                            .Stop("english_stop", m => m.StopWords(_elasticsearchSettings.StopWords))
                            .Stop("english_stop_freetext", m => m.StopWords(_elasticsearchSettings.StopWordsExtended))
                            .Stemmer("english_stemmer", m => m.Language("english"))
                            .PatternReplace("pattern_replace_er", m => m.Pattern("or\b").Replacement("er"))
                            .Synonym("english_custom_synonyms", s => s.Synonyms(_elasticsearchSettings.Synonyms)))
                        .Analyzers(a => a
                            .Custom(AnalyserEnglishCustom, l => l
                                .Tokenizer("standard")
                                .Filters("english_possessive_stemmer", "lowercase", "english_stop", "english_custom_synonyms", "pattern_replace_er", "english_stemmer")
                                .CharFilters("char_pattern_replace_er"))
                            .Custom(AnalyserEnglishCustomText, l => l
                                .Tokenizer("standard")
                                .Filters("english_possessive_stemmer", "lowercase", "english_stop_freetext", "english_custom_synonyms")));
        }

        public int ApprenticeshipIndexShards() => !string.IsNullOrEmpty(_elasticsearchSettings.ApprenticeshipIndexShards) ? int.Parse(_elasticsearchSettings.ApprenticeshipIndexShards) : 1;


        public int ApprenticeshipIndexReplicas() => !string.IsNullOrEmpty(_elasticsearchSettings.ApprenticeshipIndexShards) ? int.Parse(_elasticsearchSettings.ApprenticeshipIndexReplicas) : 0;

        public int ProviderIndexShards() => !string.IsNullOrEmpty(_elasticsearchSettings.ApprenticeshipIndexShards) ? int.Parse(_elasticsearchSettings.ProviderIndexShards) : 1;

        public int ProviderIndexReplicas() => !string.IsNullOrEmpty(_elasticsearchSettings.ApprenticeshipIndexShards) ? int.Parse(_elasticsearchSettings.ProviderIndexReplicas) : 0;
    }
}
