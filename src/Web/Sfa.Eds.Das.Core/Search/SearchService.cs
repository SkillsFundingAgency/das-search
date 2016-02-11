namespace Sfa.Eds.Das.Core.Search
{
    using System;
    using System.Linq;

    using Interfaces.Search;
    using Models;

    using Interfaces;

    using log4net;

    public class SearchService : ISearchService
    {
        private readonly IElasticsearchClientFactory elasticsearchClientFactory;

        private readonly ILog logging;

        public SearchService(IElasticsearchClientFactory elasticsearchClientFactory, ILog logging)
        {
            this.elasticsearchClientFactory = elasticsearchClientFactory;
            this.logging = logging;
        }

        public SearchResults SearchByKeyword(string keywords, int skip, int take)
        {
            var t = take == 0 ? 1000 : take;

            var client = this.elasticsearchClientFactory.Create();
            
            var results = client.Search<SearchResultsItem>(s => s
            .Skip(skip)
            .Take(t)
            .QueryString(QueryHelper.FormatQuery(keywords)));

            var documents = results.Documents.Where(i => !string.IsNullOrEmpty(i.Title)).ToList();

            return new SearchResults
            {
                TotalResults = results.Total,
                SearchTerm = keywords,
                Results = documents,
                HasError = results.ConnectionStatus.HttpStatusCode != 200
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

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                this.logging.Error($"Trying to get standard with id {standardId}");
                throw new ApplicationException($"Failed query standard with id {standardId}");
            }

            return results.Documents.Any() ? results.Documents.First() : null;
        }
    }

}