namespace Sfa.Eds.Das.Infrastructure.ElasticSearch
{
    using System.Linq;

    using Sfa.Das.ApplicationServices;
    using Sfa.Das.ApplicationServices.Models;
    using Sfa.Eds.Das.Core.Configuration;

    public sealed class ElasticsearchProvider : ISearchProvider
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;
        private readonly IConfigurationSettings _applicationSettings;

        public ElasticsearchProvider(IElasticsearchClientFactory elasticsearchClientFactory, IConfigurationSettings applicationSettings)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationSettings = applicationSettings;
        }

        public StandardSearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            keywords = QueryHelper.FormatQuery(keywords);

            var client = this._elasticsearchClientFactory.Create();
            var results = client.Search<StandardSearchResultsItem>(s => s.Skip(skip).Take(take).QueryString(keywords));

            return new StandardSearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = results.Documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }

        public ProviderSearchResults SearchByStandardId(string standardId, int skip, int take)
        {
            var client = this._elasticsearchClientFactory.Create();

            var results = client
                .Search<ProviderSearchResultsItem>(s => s
                    .Index(_applicationSettings.ProviderIndexAlias)
                    .MatchAll()
                    .Filter(f => f
                        .Term(y => y.StandardsId, standardId)));

            var documents = results.Documents.Where(i => !string.IsNullOrEmpty(i.UkPrn)).OrderBy(x => x.ProviderName);

            string standardName = string.Empty;

            var standardSearchResultsItem = client
                .Search<StandardSearchResultsItem>(s => s
                    .Index(_applicationSettings.StandardIndexesAlias)
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs =>
                            qs.OnFields(e => e.StandardId)
                                .Query(standardId)))).Documents.FirstOrDefault();

            if (standardSearchResultsItem != null)
            {
                standardName = standardSearchResultsItem.Title;
            }

            return new ProviderSearchResults
            {
                TotalResults = results.Total,
                StandardId = int.Parse(standardId),
                StandardName = standardName,
                Results = documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
            };
        }
    }
}