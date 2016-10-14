using System.Collections.Generic;
using Sfa.Das.ApprenticeshipInfoService.Core.Logging;
using Sfa.Das.ApprenticeshipInfoService.Core.Models.Responses;
using SFA.DAS.Apprenticeships.Api.Types;

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
        private readonly IProviderMapping _providerMapping;

        public ProviderRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IProviderLocationSearchProvider providerLocationSearchProvider,
            IProviderMapping providerMapping)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _providerLocationSearchProvider = providerLocationSearchProvider;
            _providerMapping = providerMapping;
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

        public List<StandardProviderSearchResultsItemResponse> GetByStandardIdAndLocation(int id, double lat, double lon, int page)
        {
            var coordinates = new Coordinate
            {
                Lat = lat,
                Lon = lon
            };

            var providers = _providerLocationSearchProvider.SearchStandardProviders(id, coordinates, page);

            return providers.Select(provider => _providerMapping.MapToStandardProviderResponse(provider)).ToList();
        }

        public List<FrameworkProviderSearchResultsItemResponse> GetByFrameworkIdAndLocation(int id, double lat, double lon, int page)
        {
            var coordinates = new Coordinate
            {
                Lat = lat,
                Lon = lon
            };

            var providers = _providerLocationSearchProvider.SearchFrameworkProviders(id, coordinates, page);

            return providers.Select(provider => _providerMapping.MapToFrameworkProviderResponse(provider)).ToList();
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
