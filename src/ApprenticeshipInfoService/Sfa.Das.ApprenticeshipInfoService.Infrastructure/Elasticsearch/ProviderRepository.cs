using System.Collections.Generic;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;

namespace Sfa.Das.ApprenticeshipInfoService.Infrastructure.Elasticsearch
{
    using System;
    using System.Linq;
    using Nest;
    using Sfa.Das.ApprenticeshipInfoService.Core.Configuration;
    using Sfa.Das.ApprenticeshipInfoService.Core.Models;
    using Sfa.Das.ApprenticeshipInfoService.Core.Services;
    using Sfa.Das.ApprenticeshipInfoService.Infrastructure.Mapping;

    public sealed class ProviderRepository : IGetProviders
    {
        private readonly IElasticsearchCustomClient _elasticsearchCustomClient;
        private readonly ILog _applicationLogger;
        private readonly IConfigurationSettings _applicationSettings;
        private readonly IProviderLocationSearchProvider _providerLocationSearchProvider;

        public ProviderRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderLocationSearchProvider providerLocationSearchProvider)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerLocationSearchProvider = providerLocationSearchProvider;
        }

        public IEnumerable<Provider> GetAllProviders()
        {
            var take = GetProvidersTotalAmount();
            var results =
                _elasticsearchCustomClient.Search<Provider>(
                    s =>
                    s.Index(_applicationSettings.ProviderIndexAlias)
                        .Type(Types.Parse("providerdocument"))
                        .From(0)
                        .Sort(sort => sort.Ascending(f => f.Ukprn))
                        .Take(take)
                        .MatchAll());

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query all standards");
            }

            return results.Documents;
        }

        public IEnumerable<Provider> GetProvidersByUkprn(int ukprn)
        {
            var results =
                _elasticsearchCustomClient.Search<Provider>(
                    s =>
                    s.Index(_applicationSettings.ProviderIndexAlias)
                        .Type(Types.Parse("providerdocument"))
                        .From(0)
                        .Sort(sort => sort.Ascending(f => f.Ukprn))
                        .Take(100)
                        .Query(q => q
                            .Terms(t => t
                                .Field(f => f.Ukprn)
                                .Terms(ukprn))));

            if (results.ApiCall.HttpStatusCode != 200)
            {
                throw new ApplicationException($"Failed query all standards");
            }

            return results.Documents;
        }

        public List<StandardProviderSearchResultsItem> GetByStandardIdAndLocation(int id, double lat, double lon, int page)
        {
            var coordinates = new Coordinate
            {
                Lat = lat,
                Lon = lon
            };

            return _providerLocationSearchProvider.SearchStandardProviders(id, coordinates, page);
        }

        public List<FrameworkProviderSearchResultsItem> GetByFrameworkIdAndLocation(int id, double lat, double lon, int page)
        {
            var coordinates = new Coordinate
            {
                Lat = lat,
                Lon = lon
            };

            return _providerLocationSearchProvider.SearchFrameworkProviders(id, coordinates, page);
        }

        private int GetProvidersTotalAmount()
        {
            var results =
                _elasticsearchCustomClient.Search<Provider>(
                    s =>
                    s.Index(_applicationSettings.ProviderIndexAlias)
                        .Type(Types.Parse("providerdocument"))
                        .From(0)
                        .MatchAll());
            return (int)results.HitsMetaData.Total;
        }
    }
}
