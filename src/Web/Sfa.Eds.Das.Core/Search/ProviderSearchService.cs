namespace Sfa.Eds.Das.Core.Search
{
    using System;
    using System.Linq;

    using Interfaces.Search;
    using Models;

    using Interfaces;

    using log4net;

    public class ProviderSearchService : IProviderSearchService
    {
        private readonly IElasticsearchClientFactory elasticsearchClientFactory;

        private readonly ILog logging;

        private readonly IApplicationSettings _applicationSettings;


        public ProviderSearchService(IElasticsearchClientFactory elasticsearchClientFactory, ILog logging, IApplicationSettings applicationSettings)
        {
            this.elasticsearchClientFactory = elasticsearchClientFactory;
            this.logging = logging;
            _applicationSettings = applicationSettings;
        }

        public ProviderSearchResults SearchByStandardId(string standardId, int skip, int take)
        {
            var t = take == 0 ? 1000 : take;

            var client = this.elasticsearchClientFactory.Create();
            
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

        public StandardSearchResultsItem GetStandardItem(string standardId)
        {
            var client = this.elasticsearchClientFactory.Create();

            var results =
                client.Search<StandardSearchResultsItem>(
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