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
        private readonly IGetStandards _getStandards;
        private readonly IProviderLocationSearchProvider _providerLocationSearchProvider;
        private readonly IStandardMapping _standardMapping;

        public ProviderRepository(
            IElasticsearchCustomClient elasticsearchCustomClient,
            ILog applicationLogger,
            IConfigurationSettings applicationSettings,
            IGetStandards getStandards,
            IProviderLocationSearchProvider providerLocationSearchProvider)
        {
            _elasticsearchCustomClient = elasticsearchCustomClient;
            _applicationLogger = applicationLogger;
            _applicationSettings = applicationSettings;
            _getStandards = getStandards;
            _providerLocationSearchProvider = providerLocationSearchProvider;
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
    }
}
