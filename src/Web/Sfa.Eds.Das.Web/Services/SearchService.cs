namespace Sfa.Eds.Das.Web.Services
{
    using System.Linq;

    using Sfa.Eds.Das.Web.Models;
    using Sfa.Eds.Das.Web.Services.Factories;

    public class SearchService : ISearchForStandards
    {
        private readonly IElasticsearchClientFactory elasticsearchClientFactory;

        public SearchService(IElasticsearchClientFactory elasticsearchClientFactory)
        {
            this.elasticsearchClientFactory = elasticsearchClientFactory;
        }

        public SearchResults Search(string keywords)
        {
            var client = elasticsearchClientFactory.Create();

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
            var client = elasticsearchClientFactory.Create();

            var results =
                client.Search<SearchResultsItem>(
                    s => s
                    .From(0)
                    .Size(1000)
                    .Query(q =>
                        q.QueryString(qs =>
                            qs.OnFields(e => e.StandardId)
                            .Query(standardId)))
                    );

            return results.Documents.Any() ? results.Documents.First() : null;
        }

    }
}