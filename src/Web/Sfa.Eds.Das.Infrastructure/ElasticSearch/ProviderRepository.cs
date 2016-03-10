namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Core.Domain.Services;
    using Core.Logging;
    using ElasticSearch;
    using Sfa.Das.ApplicationServices.Models;

    public sealed class ProviderRepository : IProviderRepository
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _applicationLogger;

        public ProviderRepository(IElasticsearchClientFactory elasticsearchClientFactory, ILog applicationLogger)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
        }

        public Provider GetById(string providerid, string locationId, string standardCode)
        {
            var client = _elasticsearchClientFactory.Create();

            var results =
               client.Search<ProviderSearchResultsItem>(
                   s => s
                   .Index("integrationproviderindexesalias-2000-01-01-00")
                   .From(0)
                   .Size(1)
                   .Query(q => q
                       .Term(t => t.Id, providerid)
                        && q.Term(t => t.LocationId, locationId)
                        && q.Term("standardcode", standardCode) // t.StandardCode is generated as standardCode and the JSON property is all lowercase (standardcode) -> Fix it or D..
                       ));

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get standard with id {providerid}");

                throw new ApplicationException($"Failed query standard with id {providerid}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document == null)
            {
                return null;
            }

            return new Provider
            {
                Id = document.Id,
                ProviderName = document.Name,
                VenueName = document.LocationName,
                UkPrn = document.UkPrn
            };
        }
    }
}
