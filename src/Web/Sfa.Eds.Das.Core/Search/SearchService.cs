namespace Sfa.Eds.Das.Core.Search
{
    using System.Linq;

    using Interfaces.Search;
    using Models;

    using Interfaces;

    public class SearchService : ISearchService
    {
        private readonly IElasticsearchClientFactory elasticsearchClientFactory;

        public SearchService(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            this.elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public SearchResults SearchByKeyword(string keywords)
        {
            var client = this.elasticsearchClientFactory.Create();

            var results = client.Search<SearchResultsItem>(s => s
            .From(0)
            .Size(1000)
            .QueryString(keywords));

            return new SearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = results.Documents.Where(i => !string.IsNullOrEmpty(i.Title))
            };
        }

        public SearchResultsItem GetStandardItem(string standardId)
        {
            var client = this.elasticsearchClientFactory.Create();

            var results =
                client.Search<SearchResultsItem>(
                    s => s
                    .From(0)
                    .Size(1)
                    .Query(q =>
                        q.QueryString(qs =>
                            qs.OnFields(e => e.StandardId)
                            .Query(standardId))));
            return results.Documents.Any() ? results.Documents.First() : null;
        }
    }
}