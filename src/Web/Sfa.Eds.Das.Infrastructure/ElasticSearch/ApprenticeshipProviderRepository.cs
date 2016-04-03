using Sfa.Eds.Das.Core.Configuration;
using Sfa.Eds.Das.Infrastructure.Mapping;

namespace Sfa.Eds.Das.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;

    using Core.Domain.Model;
    using Core.Domain.Services;
    using Core.Logging;
    using ElasticSearch;
    using Sfa.Das.ApplicationServices.Models;

    public sealed class ApprenticeshipProviderRepository : IApprenticeshipProviderRepository
    {
        private readonly IElasticsearchClientFactory _elasticsearchClientFactory;

        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IProviderMapping _providerMapping;

        public ApprenticeshipProviderRepository(
            IElasticsearchClientFactory elasticsearchClientFactory,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderMapping providerMapping)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerMapping = providerMapping;
        }

        public Provider GetById(string providerid, string locationId, string standardCode)
        {
            var client = _elasticsearchClientFactory.Create();

            var results =
               client.Search<StandardProviderSearchResultsItem>(s => s
                   .Index(_applicationSettings.ProviderIndexAlias)
                   .From(0)
                   .Size(1)
                   .Query(q => q
                       .Term(t => t.Id, providerid)
                        && q.Term(t => t.LocationId, locationId)
                        && q.Term(t => t.StandardCode, standardCode)));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get standard with provider id {providerid}, standard code {standardCode} and location id {locationId}");
                throw new ApplicationException($"Failed query standard with provider {providerid}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document == null)
            {
                return null;
            }

            return _providerMapping.MapToProvider(document);
        }
    }
}
