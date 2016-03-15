using Sfa.Eds.Das.Core.Configuration;

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
        private readonly IConfigurationSettings _applicationSettings;

        public ProviderRepository(IElasticsearchClientFactory elasticsearchClientFactory, ILog applicationLogger, IConfigurationSettings applicationSettings)
        {
            _elasticsearchClientFactory = elasticsearchClientFactory;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
        }

        public Provider GetById(string providerid, string locationId, string standardCode)
        {
            var client = _elasticsearchClientFactory.Create();

            var results =
               client.Search<ProviderSearchResultsItem>(s => s
                   .Index(_applicationSettings.ProviderIndexAlias)
                   .From(0)
                   .Size(1)
                   .Query(q => q
                       .Term(t => t.Id, providerid)
                        && q.Term(t => t.LocationId, locationId)
                        && q.Term(t => t.StandardCode, standardCode)));

            if (results.ConnectionStatus.HttpStatusCode != 200)
            {
                _applicationLogger.Error($"Trying to get standard with provider id {providerid}, standard code {standardCode} and location id {locationId}");

                throw new ApplicationException($"Failed query standard with provider {providerid}");
            }

            var document = results.Documents.Any() ? results.Documents.First() : null;

            if (document == null)
            {
                return null;
            }

            return new Provider
            {
                Id = document.Id,
                Address = document.Address,
                DeliveryModes = document.DeliveryModes,
                UkPrn = document.UkPrn,
                Phone = document.Phone,
                Email = document.Email,
                Website = document.Website,
                LearnerSatisfaction = document.LearnerSatisfaction * 10,
                EmployerSatisfaction = document.EmployerSatisfaction * 10,
                StandardInfoUrl = document.StandardInfoUrl,
                LocationName = document.LocationName,
                Name = document.Name,
                ContactUsUrl = document.ContactUsUrl,
                LocationId = document.LocationId,
                StandardCode = document.StandardCode,
                MarketingName = document.MarketingName,
                Distance = document.Distance
            };
        }
    }
}
